using System;
using System.IO;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Core.DependencyInjection;
using Annium.Id.Api.TestClient;
using Annium.Id.Demo.TestClient;
using Annium.Net.Http;
using Annium.Net.Mail;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Api.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        #region id

        public IHttpRequest IdApi => GetRequest<Startup>(
            builder => builder.UseServicePack<TestServicePack>(),
            services => services.AddSingleton<IEmailService>(emailService)
        );

        public ExtendedClient Id() => IdApi.ApiClient(emailService);

        public ExtendedClient Id(string token) => IdApi.BearerAuthorization(token).ApiClient(emailService);

        protected readonly TestEmailService emailService = new TestEmailService();

        #endregion

        #region demo

        public IHttpRequest DemoApi(Guid appId) => GetRequest<Demo.Startup>(
            builder => builder.UseServicePack<Demo.ServicePack>(),
            services => services
                .AddIdAuthorization(options =>
                {
                    options.Audience = appId;
                    options.PublicKeyFile = Path.Combine("keys", "public.key");
                    options.AccessTokenLifeTime = Duration.FromMinutes(5);
                    options.RefreshTokenLifeTime = Duration.FromMinutes(5);
                })
        );

        public Demo.TestClient.Client Demo(Guid appId) => DemoApi(appId).DemoClient();

        public Demo.TestClient.Client Demo(Guid appId, string token) => DemoApi(appId).BearerAuthorization(token).DemoClient();

        #endregion
    }
}