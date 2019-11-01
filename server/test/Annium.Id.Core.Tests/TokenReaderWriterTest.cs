using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Annium.Core.DependencyInjection;
using Annium.Data.Operations;
using Annium.Testing;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Core.Tests
{
    public class TokenReaderWriterTest
    {
        [Fact]
        public void Read_InvalidJWT_ReturnsBadSource()
        {
            // arrange
            var token = "badtoken";

            // act
            var (status, _) = ReadToken(token);

            // assert
            status.IsEqual(TokenReadStatus.BadSource);
        }

        [Fact]
        public void Read_Expired_ReturnsFailure()
        {
            // arrange
            var source = GenerateToken();
            var token = WriteToken(source, expired: true);

            // act
            var result = ReadToken(token);

            // assert
            result.Status.IsEqual(TokenReadStatus.Failed);
            result.PlainErrors.Any(x => x.Contains("expired")).IsTrue();
        }

        [Fact]
        public void Read_InvalidAudience_ReturnsFailure()
        {
            // arrange
            var source = GenerateToken();
            var token = WriteToken(source, audience: "other");

            // act
            var result = ReadToken(token);

            // assert
            result.Status.IsEqual(TokenReadStatus.Failed);
            result.PlainErrors.Any(x => x.Contains("audience")).IsTrue();
        }

        [Fact]
        public void Read_Valid_ReturnsToken()
        {
            // arrange
            var source = GenerateToken();
            var token = WriteToken(source);

            // act
            var (status, result) = ReadToken(token);

            // assert
            status.IsEqual(TokenReadStatus.Ok);
            Serialize(result).IsEqual(Serialize(source));
        }

        private string WriteToken(
            IdToken token,
            bool expired = false,
            string audience = "demo"
        )
        {
            var services = new ServiceCollection();
            services.AddIdAuthorizationCoreServices(Configure(audience));
            if (expired)
                services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant() - Duration.FromDays(1));
            else
                services.AddSingleton<Func<Instant>>(SystemClock.Instance.GetCurrentInstant);
            var provider = services.BuildServiceProvider();

            var writer = provider.GetRequiredService<ITokenWriter>();

            return writer.WriteToken(token, audience);
        }

        private IStatusResult<TokenReadStatus, IdToken> ReadToken(string token)
        {
            var services = new ServiceCollection();
            services.AddIdAuthorizationCoreServices(Configure("demo"));
            services.AddSingleton<Func<Instant>>(SystemClock.Instance.GetCurrentInstant);
            services.AddLogging(route => route.UseConsole());
            var provider = services.BuildServiceProvider();

            var reader = provider.GetRequiredService<ITokenReader>();

            return reader.ReadToken(token, new TokenReadOptions());
        }

        private Action<AuthOptions> Configure(string audience)
        {
            return options =>
            {
                options.Audience = audience;
                options.PrivateKeyFile = Path.Combine("keys", "private.key");
                options.PublicKeyFile = Path.Combine("keys", "public.key");
                options.AccessTokenLifeTime = Duration.FromMinutes(1);
            };
        }

        private IdToken GenerateToken(string app = "demo")
        {
            return new IdToken(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new AppToken(
                    Guid.NewGuid(),
                     app,
                      Guid.NewGuid(),
                      new[] { "user", "skilled" },
                      new Dictionary<string, string> { { "moderate", "newbies" }, { "advice", "all" } }
                ),
                new[]
                {
                    new CompanyToken(
                        Guid.NewGuid(),
                        "one",
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
}