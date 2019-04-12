using System;
using Annium.Extensions.DependencyInjection;
using Annium.Id.AspNetCore.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.AspNetCore
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // tools
            services.AddSingleton<IIdentityDataAccessor, IdentityDataAccessor>();
        }
    }
}