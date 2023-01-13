using System;
using Annium.Core.DependencyInjection;

namespace Site;

public class ServicePack : ServicePackBase
{
    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        // core
        container.AddTime().WithRealTime().SetDefault();
        container.AddRuntime(GetType().Assembly);
        container.AddMapper();
        container.AddHttpRequestFactory();
        container.AddComponentFormStateFactory();
        container.AddValidation();
        container.AddLocalization(opts => opts.UseInMemoryStorage());
        container.AddCss();
        container.AddHostHttpRequestFactory();
        // container.AddLogging(route => route.UseConsole());

        // app
        container.AddAntDesign();
        container.AddSerializers()
            .WithJson(opts => opts.ConfigureForOperations().ConfigureForNodaTime(), isDefault: true);
        container.Add<Theme>().AsSelf().Singleton();
        container.AddStorages();
        container.AddApiServices();
    }
}