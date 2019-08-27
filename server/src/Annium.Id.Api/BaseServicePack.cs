using System;
using Annium.Core.DependencyInjection;
using Annium.Extensions.DependencyInjection;
using Annium.Id.Api.Tools;
using Annium.Logging.Abstractions;
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
            services.AddIdAuthorization();

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // tools
            services.AddSingleton<IIdentityDataAccessor, IdentityDataAccessor>();
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            services.AddSingleton(new LoggerConfiguration(LogLevel.Trace));
            services.AddConsoleLogger();
            services.AddLocalization(opts => opts.UseYamlStorage());
            services.AddValidation();
            services.AddMapper();
        }
    }
}