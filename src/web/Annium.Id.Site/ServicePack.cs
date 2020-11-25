using System;
using Annium.Blazor.Css;
using Annium.Core.DependencyInjection;
using Annium.Core.Runtime.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Site
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            // core
            container.AddTimeProvider();
            container.AddRuntimeTools(GetType().Assembly, false);
            container.AddMapper();
            container.AddHttpRequestFactory();
            container.AddComponentFormStateFactory();
            container.AddValidation();
            container.AddLocalization(opts => opts.UseInMemoryStorage());
            container.AddCssRules();
            container.AddHostHttpRequestFactory();
            // container.AddLogging(route => route.UseConsole());

            // app
            container.Collection.AddAntDesign();
            container.AddJsonSerializers((sp, opts) => opts
                .ConfigureDefault(sp.Resolve<ITypeManager>())
                .ConfigureForOperations()
                .ConfigureForNodaTime());
            container.Add<Theme>().AsSelf().Singleton();
            container.AddStorages();
            container.AddApiServices();
        }
    }
}