using System;
using System.Linq;
using Annium.Core.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Annium.Id.Api
{
    public class Startup<TServicePack> where TServicePack : ServicePackBase, new()
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc()
                .AddJsonOptions(opts => opts.SerializerSettings
                    .ConfigureForNodaTime(DateTimeZoneProviders.Serialization)
                    .ConfigureForOperations()
                );

            services.AddOpenApiDocument(doc =>
            {
                doc.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                });

                doc.OperationProcessors
                    .Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

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