using Annium.Core.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Demo;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddControllers()
            .AddDefaultJsonOptions();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseExceptionMiddleware();
        app.UseXRest();
        app.UseRouting();
        app.UseCors(builder => builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}