using System;
using System.Threading.Tasks;
using Annium.Configuration.Abstractions;
using Annium.Configuration.Yaml;
using Annium.Core.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Site.Shared;

namespace Site;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("app");
        builder.ConfigureContainer(new ServiceProviderFactory(x => x.UseServicePack<ServicePack>()));
        var container = new ServiceContainer(builder.Services);
        await container.AddConfigurationAsync<Configuration>(cfg =>
            cfg.AddRemoteYaml(new Uri($"{builder.HostEnvironment.BaseAddress}site.yml"))
        );
        // builder.Logging.ConfigureLoggingBridge();
        await builder.Build().RunAsync();
    }
}
