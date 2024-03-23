using System;
using System.Linq;
using Annium.Core.DependencyInjection;
using Annium.Net.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Server.DemoHost;

public class ServicePack : ServicePackBase
{
    public override void Configure(IServiceContainer container)
    {
        container.AddRuntime(GetType().Assembly);
        container.AddConfiguration(new WebHostConfiguration());
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.AddTime().WithRealTime().SetDefault();
        container.AddMapper();
        container
            .AddSerializers()
            .WithJson(opts => opts.ConfigureForOperations().ConfigureForNodaTime(), isDefault: true);
        container.AddHttpRequestFactory(true);
        container.AddLogging();
        container.AddXRest();

        // auth
        // FIXME: removed, cause id is set dynamically from tests
        // container.AddIdAuthorization(options =>
        // {
        //     options.Audience = Constants.AppId;
        //     options.PublicKeyFile = Path.Combine("keys", "public.key");
        // });
        container.AddIdPolicy("isAdmin", token => Enumerable.Contains(token.App.Roles, "admin"));
        container.AddIdPolicy(
            "hasPaymentsAccess",
            token => token.App.Claims.ContainsKey("paymentsAccess") && token.App.Claims["paymentsAccess"] == "full"
        );
        container.AddIdPolicy<Guid>(
            "hasCompanyPaymentsAccess",
            (token, companyId) =>
                token.Companies.Any(c =>
                    c.Id == companyId && c.Claims.ContainsKey("paymentsAccess") && c.Claims["paymentsAccess"] == "full"
                )
        );

        // host
        container.Collection.AddCors();
        container.Collection.AddControllers().AddDefaultJsonOptions();
    }

    public override void Setup(IServiceProvider provider)
    {
        provider.UseLogging(route => route.UseConsole());
        SetupNetTypes(provider.Resolve<IMapperConfig>());
    }

    private void SetupNetTypes(IMapperConfig config)
    {
        config.Exclude(Match.NamespaceStartsWith("Server.DemoHost"));
    }
}
