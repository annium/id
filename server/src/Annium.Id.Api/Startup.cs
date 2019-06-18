using System;
using Annium.Data.Operations.Serialization;
using Annium.Extensions.DependencyInjection;
using Annium.Id.Api.Payloads;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NSwag.AspNetCore;

namespace Annium.Id.Api
{
    public class Startup<TServicePack> where TServicePack : ServicePackBase, new()
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc()
                .AddDataAnnotationsLocalization(opts =>
                    opts.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(Annotations)))
                .AddJsonOptions(opts => opts.SerializerSettings
                    .ConfigureForNodaTime(DateTimeZoneProviders.Serialization)
                    .ConfigureForOperations()
                );

            services.AddSwaggerDocument();

            return new ServiceProviderBuilder(services)
                .UseServicePack<TServicePack>()
                .Build();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime, IHostingEnvironment env)
        {
            app.UseExceptionMiddleware();

            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseCors(builder => builder
                .SetIsOriginAllowed(o => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseRequestLocalization("en", "ru");

            app.UseMvc();
        }
    }
}