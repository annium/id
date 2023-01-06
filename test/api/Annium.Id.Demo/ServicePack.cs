using System;
using System.Linq;
using Annium.Core.DependencyInjection;

namespace Annium.Id.Demo;

public class ServicePack : ServicePackBase
{
    public override void Configure(IServiceContainer container)
    {
        // register configurations
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.AddRuntime(GetType().Assembly);

        container.AddTime().WithRealTime().SetDefault();
        container.AddSerializers()
            .WithJson(opts => opts.ConfigureForOperations().ConfigureForNodaTime(), isDefault: true);
        container.AddHttpRequestFactory().SetDefault();
        container.AddXRest();

        // FIXME: removed, cause id is set dynamically from tests
        // container.AddIdAuthorization(options =>
        // {
        //     options.Audience = Constants.AppId;
        //     options.PublicKeyFile = Path.Combine("keys", "public.key");
        // });
        container.AddLogging();
        container.AddIdPolicy(
            "isAdmin",
            token => token.App.Roles.Contains("admin")
        );
        container.AddIdPolicy(
            "hasPaymentsAccess",
            token => token.App.Claims.ContainsKey("paymentsAccess") &&
                token.App.Claims["paymentsAccess"] == "full"
        );
        container.AddIdPolicy<Guid>(
            "hasCompanyPaymentsAccess",
            (token, companyId) => token.Companies.Any(
                c => c.Id == companyId && c.Claims.ContainsKey("paymentsAccess") && c.Claims["paymentsAccess"] == "full"
            )
        );
        container.AddMapper();
    }

    public override void Setup(IServiceProvider provider)
    {
        provider.UseLogging(route => route.UseConsole());
    }
}