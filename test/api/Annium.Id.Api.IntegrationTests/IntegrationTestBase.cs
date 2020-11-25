using System;
using System.IO;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Core.DependencyInjection;
using Annium.Id.Api.TestClient;
using Annium.Id.Demo.TestClient;
using Annium.Net.Http;
using Annium.Net.Mail;
using NodaTime;

namespace Annium.Id.Api.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        #region id

        public IHttpRequest IdApi => GetRequest<Startup>(
            builder => builder.UseServicePack<TestServicePack>(),
            container =>
            {
                container.AddHttpRequestFactory();
                container.Add(EmailService).AsSelf().AsInterfaces().Singleton();
            });

        public ExtendedClient Id()
        {
            return IdApi.ApiClient(EmailService);
        }

        public ExtendedClient Id(string token)
        {
            return IdApi.BearerAuthorization(token).ApiClient(EmailService);
        }

        protected readonly TestEmailService EmailService = new TestEmailService();

        #endregion

        #region demo

        public IHttpRequest DemoApi(Guid appId)
        {
            return GetRequest<Demo.Startup>(
                builder => builder.UseServicePack<Demo.ServicePack>(),
                services => services
                    .AddHttpRequestFactory()
                    .AddIdAuthorization((_, options) =>
                    {
                        options.Audience = appId;
                        options.PublicKeyFile = Path.Combine("keys", "public.key");
                        options.AccessTokenLifeTime = Duration.FromMinutes(5);
                        options.RefreshTokenLifeTime = Duration.FromMinutes(5);
                    })
            );
        }

        public Demo.TestClient.Client Demo(Guid appId)
        {
            return DemoApi(appId).DemoClient();
        }

        public Demo.TestClient.Client Demo(Guid appId, string token)
        {
            return DemoApi(appId).BearerAuthorization(token).DemoClient();
        }

        #endregion
    }
}