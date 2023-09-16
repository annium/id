using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Annium.Core.DependencyInjection;
using Annium.Core.Runtime.Time;
using Annium.Data.Operations;
using Annium.Identity.Tokens.Jwt;
using Annium.Testing;
using NodaTime;
using Xunit;

namespace Annium.Id.Core.Tests;

public class TokenReaderWriterTest
{
    [Fact]
    public void Read_InvalidJWT_ReturnsBadSource()
    {
        // arrange
        var token = "badtoken";

        // act
        var (status, _) = ReadToken(token, Guid.NewGuid());

        // assert
        status.Is(JwtReadStatus.BadSource);
    }

    [Fact]
    public void Read_Expired_ReturnsFailure()
    {
        // arrange
        var appId = Guid.NewGuid();
        var source = GenerateToken(appId);
        var token = WriteToken(source, true);

        // act
        var result = ReadToken(token, appId);

        // assert
        result.Status.Is(JwtReadStatus.Failed);
        result.PlainErrors.Any(x => x.Contains("expired")).IsTrue();
    }

    [Fact]
    public void Read_InvalidAudience_ReturnsFailure()
    {
        // arrange
        var appId = Guid.NewGuid();
        var source = GenerateToken(appId);
        var token = WriteToken(source);

        // act
        var result = ReadToken(token, Guid.NewGuid());

        // assert
        result.Status.Is(JwtReadStatus.Failed);
        result.PlainErrors.Any(x => x.Contains("audience")).IsTrue();
    }

    [Fact]
    public void Read_Valid_ReturnsToken()
    {
        // arrange
        var appId = Guid.NewGuid();
        var source = GenerateToken(appId);
        var token = WriteToken(source);

        // act
        var (status, result) = ReadToken(token, appId);

        // assert
        status.Is(JwtReadStatus.Ok);
        Serialize(result).Is(Serialize(source));
    }

    private string WriteToken(
        IdToken token,
        bool expired = false
    )
    {
        var container = new ServiceContainer();
        container.AddIdAuthorizationCoreServices(Configure(token.App.Id));
        container.AddTime().WithManagedTime().SetDefault();

        var provider = container.BuildServiceProvider();

        var timeManager = provider.Resolve<ITimeManager>();
        if (expired)
            timeManager.SetNow(SystemClock.Instance.GetCurrentInstant() - Duration.FromDays(1));
        else
            timeManager.SetNow(SystemClock.Instance.GetCurrentInstant());

        var writer = provider.Resolve<ITokenWriter>();

        return writer.WriteToken(token);
    }

    private IStatusResult<JwtReadStatus, IdToken> ReadToken(string token, Guid appId)
    {
        var container = new ServiceContainer();
        container.AddIdAuthorizationCoreServices(Configure(appId));
        container.AddTime().WithRealTime().SetDefault();
        container.AddLogging();

        var provider = container.BuildServiceProvider();
        provider.UseLogging(route => route.UseConsole());

        var reader = provider.Resolve<ITokenReader>();

        return reader.ReadToken(token, new TokenReadOptions());
    }

    private Action<IServiceProvider, AuthOptions> Configure(Guid appId)
    {
        return (_, options) =>
        {
            options.Audience = appId;
            options.PrivateKeyFile = Path.Combine("keys", "private.key");
            options.PublicKeyFile = Path.Combine("keys", "public.key");
            options.AccessTokenLifeTime = Duration.FromMinutes(1);
        };
    }

    private IdToken GenerateToken(Guid appId)
    {
        return new IdToken(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new AppToken(
                appId,
                new[] { "user", "skilled" },
                new Dictionary<string, string> { { "moderate", "newbies" }, { "advice", "all" } }
            ),
            new[]
            {
                new CompanyToken(
                    Guid.NewGuid(),
                    new[] { "stuff", "manager" },
                    new Dictionary<string, string> { { "billing", "yes" }, { "hr", "yes" } }
                )
            }
        );
    }

    private string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions().ConfigureForOperations());
    }
}