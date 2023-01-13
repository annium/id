using System;
using System.IO;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Id.AspNetCore;
using Annium.Net.Http;
using Annium.Net.Mail;
using NodaTime;
using Server.DemoHost;
using Server.DemoHost.TestClient;
using Server.Host;
using Server.TestClient;
using ServicePack = Server.DemoHost.ServicePack;

namespace Server.IntegrationTests;

public class IntegrationTestBase : IntegrationTest
{
    #region id

    public IHttpRequest IdApi => GetAppFactory<Api>(
        builder => builder.UseServicePack<TestServicePack>(),
        container => { container.Add(EmailService).AsSelf().AsInterfaces().Singleton(); }
    ).GetHttpRequest();

    public ExtendedClient Id()
    {
        return IdApi.ApiClient(EmailService);
    }

    public ExtendedClient Id(string token)
    {
        return IdApi.BearerAuthorization(token).ApiClient(EmailService);
    }

    protected readonly TestEmailService EmailService = new();

    #endregion

    #region demo

    public IHttpRequest DemoApi(Guid appId)
    {
        return GetAppFactory<Demo>(
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
    }

    public Client Demo(Guid appId)
    {
        return DemoApi(appId).DemoClient();
    }

    public Client Demo(Guid appId, string token)
    {
        return DemoApi(appId).BearerAuthorization(token).DemoClient();
    }

    #endregion
}