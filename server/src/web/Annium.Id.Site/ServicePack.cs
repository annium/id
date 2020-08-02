using System;
using Annium.Core.DependencyInjection;
using Annium.Net.Http;
using BlazorStyled;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Site
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddRuntimeTools(GetType().Assembly);
            services.AddBlazorStyled();
            services.AddTransient<Func<IHttpRequest>>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpRequestFactory>();
                var env = sp.GetRequiredService<IWebAssemblyHostEnvironment>();
                var request = factory.Get(env.BaseAddress);

                return () => request.Clone();
            });

            services.AddMapper();
        }
    }
}