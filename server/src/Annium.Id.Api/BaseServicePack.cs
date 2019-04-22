using System;
using Annium.Extensions.DependencyInjection;
using Annium.Extensions.Localization;
using Annium.Extensions.Mapper;
using Annium.Id.Api.AppAuth;
using Annium.Id.Api.Tools;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Api
{
    public class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // auth
            services.AddAppAuthorization();
            services.AddIdAuthorization();

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // tools
            services.AddSingleton<IIdentityDataAccessor, IdentityDataAccessor>();
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            services.AddYamlLocalization();

            services.AddMapper(provider, statically : true);
        }
    }
}