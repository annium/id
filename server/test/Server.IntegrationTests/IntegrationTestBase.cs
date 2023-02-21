using System;
using System.IO;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Core.DependencyInjection;
using Annium.Id.AspNetCore;
using Annium.Net.Http;
using Annium.Net.Mail;
using Bogus;
using NodaTime;
using Server.DemoHost;
using Server.DemoHost.TestClient;
using Server.Host;
using Server.Host.TestClient.Clients;
using Xunit;
using Database = Server.IntegrationTests.Fixtures.Database;
using ServicePack = Server.DemoHost.ServicePack;

namespace Server.IntegrationTests;

public class IntegrationTestBase : IntegrationTest, IAsyncLifetime
{
    protected Faker Faker { get; } = new();

    public async Task InitializeAsync()
    {
        await Database.AcquireAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }

    #region id

    protected ExtendedClient Id() => IdApi.ApiClient(EmailService);

    protected ExtendedClient Id(string token) => IdApi.BearerAuthorization(token).ApiClient(EmailService);

    protected readonly TestEmailService EmailService = new();

    private IHttpRequest IdApi => GetAppFactory<Api>(
        builder => builder.UseServicePack<TestServicePack>(),
        ConfigureContainer
    ).GetHttpRequest();

    private void ConfigureContainer(IServiceContainer container)
    {
        container.Add(EmailService).AsSelf().AsInterfaces().Singleton();
        container.AddConfiguration(Database.Config);
    }

    #endregion

    #region demo

    protected Client Demo(Guid appId) => DemoApi(appId).DemoClient();

    protected Client Demo(Guid appId, string token) => DemoApi(appId).BearerAuthorization(token).DemoClient();

    private IHttpRequest DemoApi(Guid appId) => GetAppFactory<Demo>(
        builder => builder.UseServicePack<ServicePack>(),
        services =>
        {
            services.AddIdAuthorization((_, options) =>
            {
                options.Audience = appId;
                options.PublicKeyFile = Path.Combine("keys", "public.key");
                options.AccessTokenLifeTime = Duration.FromMinutes(5);
                options.RefreshTokenLifeTime = Duration.FromMinutes(5);
            });
        }).GetHttpRequest();

    #endregion
}