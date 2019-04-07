using System;
using Annium.Extensions.DependencyInjection;
using Annium.Extensions.Mapper;
using Annium.IdentityServer.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.IdentityServer
{
    public class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // tools
            services.AddSingleton<ISecurityManager, SecurityManager>();

            services.AddMapper(provider);
        }
    }
}