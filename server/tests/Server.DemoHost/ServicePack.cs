using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Annium.Core.Mapper;
using Annium.Core.Runtime;
using Annium.Data.Operations.Serialization.Json;
using Annium.Logging.Console;
using Annium.Logging.Shared;
using Annium.Net.Http;
using Annium.Net.Types;
using Annium.NodaTime.Serialization.Json;
using Annium.Serialization.Abstractions;
using Annium.Serialization.Json;
using Annium.XRest.Sources.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Server.DemoHost;

public class ServicePack : ServicePackBase
{
    public override Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        container.AddRuntime(GetType().Assembly);
        container.AddConfiguration(new WebHostConfiguration());

        return Task.CompletedTask;
    }

    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
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

        return Task.CompletedTask;
    }

    public override Task SetupAsync(IServiceProvider provider, CancellationToken ct)
    {
        provider.UseLogging(route => route.UseConsole());
        SetupNetTypes(provider.Resolve<IMapperConfig>());

        return Task.CompletedTask;
    }

    private void SetupNetTypes(IMapperConfig config)
    {
        config.Exclude(Match.NamespaceStartsWith("Server.DemoHost"));
    }
}
