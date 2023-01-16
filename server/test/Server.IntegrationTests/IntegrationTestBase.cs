using System;
using System.IO;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Core.DependencyInjection;
using Annium.Id.AspNetCore;
using Annium.linq2db.PostgreSql;
using Annium.Net.Http;
using Annium.Net.Mail;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using NodaTime;
using Server.DemoHost;
using Server.DemoHost.TestClient;
using Server.Host;
using Server.TestClient;
using Xunit;
using ServicePack = Server.DemoHost.ServicePack;

namespace Server.IntegrationTests;

public class IntegrationTestBase : IntegrationTest, IAsyncLifetime
{
    #region infra

    private readonly PostgreSqlConfiguration _dbConfig = new()
    {
        Database = "db",
        User = "postgres",
        Password = "postgres",
    };

    private readonly TestcontainerDatabase _db;

    #endregion

    protected IntegrationTestBase()
    {
        _db = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration("registry.annium.com/postgres:15")
            {
                Database = _dbConfig.Database,
                Username = _dbConfig.User,
                Password = _dbConfig.Password,
            })
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _db.StartAsync();
        _dbConfig.Host = _db.Hostname;
        _dbConfig.Port = _db.Port;
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _db.DisposeAsync();
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
        container.AddConfiguration(_dbConfig);
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