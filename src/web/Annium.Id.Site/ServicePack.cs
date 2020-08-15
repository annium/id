using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Runtime.Types;
using Annium.Net.Http;
using Annium.Serialization.Json;
using BlazorStyled;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
            services.AddComponentStateFactory();
            services.AddValidation();
            services.AddLocalization(opts => opts.UseInMemoryStorage());
            services.AddTransient<Func<IHttpRequest>>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpRequestFactory>();
                var env = sp.GetRequiredService<IWebAssemblyHostEnvironment>();
                var request = factory.New(env.BaseAddress);

                return () => request.Clone();
            });
            // services.AddLogging(route => route.UseConsole());

            // app
            services.AddAntDesign();
            services.AddBlazorStyled();
            services.AddSingleton(sp => StringSerializer.Configure(
                options => options
                    .ConfigureDefault(sp.GetRequiredService<ITypeManager>())
                    .ConfigureForOperations()
                    .ConfigureForNodaTime(XmlSerializationSettings.DateTimeZoneProvider)
            ));
            services.AddSingleton<Theme>();
        }
    }
}