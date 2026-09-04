using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Blazor.Ant;
using Annium.Blazor.Css;
using Annium.Blazor.Net;
using Annium.Blazor.State;
using Annium.Core.DependencyInjection;
using Annium.Core.Mapper;
using Annium.Core.Runtime;
using Annium.Data.Operations.Serialization.Json;
using Annium.Extensions.Validation;
using Annium.Localization.Abstractions;
using Annium.Localization.InMemory;
using Annium.Logging.Console;
using Annium.Logging.Shared;
using Annium.Net.Http;
using Annium.NodaTime.Serialization.Json;
using Annium.Serialization.Abstractions;
using Annium.Serialization.Json;

namespace Site;

public class ServicePack : ServicePackBase
{
    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
    {
        // core
        container.AddTime().WithRealTime().SetDefault();
        container.AddRuntime(GetType().Assembly);
        container.AddMapper();
        container.AddHttpRequestFactory();
        container.AddValidation();
        container.AddLocalization(opts => opts.UseInMemoryStorage());
        container.AddCss();
        container.AddHostHttpRequestFactory();
        // container.AddLogging(route => route.UseConsole());

        // app
        container.AddAntDesign();
        container
            .AddSerializers()
            .WithJson(opts => opts.ConfigureForOperations().ConfigureForNodaTime(), isDefault: true);
        container.Add<Theme>().AsSelf().Singleton();
        container.AddStates();
        container.AddApiServices();

        return Task.CompletedTask;
    }
}
