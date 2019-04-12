using System;
using Annium.Extensions.DependencyInjection;
using Annium.Extensions.Mapper;
using Annium.Id.Api.AppAuth;
using Annium.Id.Api.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Api
{
    public class BaseServicePack : ServicePackBase
    {
        public BaseServicePack()
        {
            Add<AspNetCore.ServicePack>();
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // app auth
            services.AddAppAuthorization();

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // tools
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            services.AddMapper(provider);
        }
    }
}