using System;
using System.Linq;
using Annium.Core.DependencyInjection;
using Annium.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.DemoClient
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

            services.AddIdAuthorization(options =>
            {
                options.Audience = "demo";
            });
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
                "isCompanyOwner",
                (token, companyId) => token.Companies.Any(
                    c => c.Id == companyId && c.OwnerId == token.UserId
                )
            );
            services.AddIdPolicy<Guid>(
                "hasCompanyPaymentsAccess",
                (token, companyId) => token.Companies.Any(
                    c => c.Id == companyId && c.Claims.ContainsKey("paymentsAccess") && c.Claims["paymentsAccess"] == "full"
                )
            );
        }

        public override void Setup(System.IServiceProvider provider)
        {
            // setup post-configured services
        }
    }
}