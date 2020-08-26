using System;
using Annium.Blazor.Css;
using Annium.Blazor.Net;
using Annium.Core.DependencyInjection;
using Annium.Core.Runtime.Types;
using Annium.Id.Site.Shared.Stores;
using Annium.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Xml;

namespace Annium.Id.Site
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            // core
            services.AddSingleton<Func<Instant>>(SystemClock.Instance.GetCurrentInstant);
            services.AddRuntimeTools(GetType().Assembly);
            services.AddMapper();
            services.AddHttpRequestFactory();
            services.AddComponentFormStateFactory();
            services.AddValidation();
            services.AddLocalization(opts => opts.UseInMemoryStorage());
            services.AddCssRules();
            services.AddHostHttpRequestFactory();
            // services.AddLogging(route => route.UseConsole());

            // app
            services.AddAntDesign();
            services.AddSingleton(sp => StringSerializer.Configure(
                options => options
                    .ConfigureDefault(sp.GetRequiredService<ITypeManager>())
                    .ConfigureForOperations()
                    .ConfigureForNodaTime(XmlSerializationSettings.DateTimeZoneProvider)
            ));
            services.AddSingleton<Theme>();
            services.AddAssemblyTypes(GetType().Assembly)
                .AssignableTo<IStore>()
                .AsSelf()
                .SingleInstance();
            services.AddApiServices();
        }
    }
}