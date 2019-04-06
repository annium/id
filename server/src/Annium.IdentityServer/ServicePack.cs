using System;
using Annium.Extensions.DependencyInjection;
using Annium.IdentityServer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.IdentityServer
{
    internal class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<Db.ServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            // register configurations
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(provider);
        }

        public override void Setup(System.IServiceProvider provider)
        {
            // setup post-configured services
        }
    }
}