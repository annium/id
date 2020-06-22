using System;
using System.Linq;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Demo
{
    public class ServicePack : ServicePackBase
    {
        public override void Configure(IServiceCollection services)
        {
            // register configurations
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(SystemClock.Instance.GetCurrentInstant);

            // FIXME: removed, cause id is set dynamically from tests
            // services.AddIdAuthorization(options =>
            // {
            //     options.Audience = Constants.AppId;
            //     options.PublicKeyFile = Path.Combine("keys", "public.key");
            // });
            services.AddLogging(route => route.UseConsole());
            services.AddIdPolicy(
                "isAdmin",
                token => token.App.Roles.Contains("admin")
            );
            services.AddIdPolicy(
                "hasPaymentsAccess",
                token => token.App.Claims.ContainsKey("paymentsAccess") &&
                token.App.Claims["paymentsAccess"] == "full"
            );
            services.AddIdPolicy<Guid>(
                "hasCompanyPaymentsAccess",
                (token, companyId) => token.Companies.Any(
                    c => c.Id == companyId && c.Claims.ContainsKey("paymentsAccess") && c.Claims["paymentsAccess"] == "full"
                )
            );
            services.AddMapper();
        }

        public override void Setup(System.IServiceProvider provider)
        {
            // setup post-configured services
        }
    }
}