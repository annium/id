using System;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Site
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            // core
            container.AddTime().WithRealTime().SetDefault();
            container.AddRuntimeTools(GetType().Assembly, false);
            container.AddMapper();
            container.AddHttpRequestFactory();
            container.AddComponentFormStateFactory();
            container.AddValidation();
            container.AddLocalization(opts => opts.UseInMemoryStorage());
            container.AddCss();
            container.AddHostHttpRequestFactory();
            // container.AddLogging(route => route.UseConsole());

            // app
            container.Collection.AddAntDesign();
            container.AddJsonSerializers()
                .Configure(opts => opts.ConfigureForOperations().ConfigureForNodaTime())
                .SetDefault();
            container.Add<Theme>().AsSelf().Singleton();
            container.AddStorages();
            container.AddApiServices();
        }
    }
}