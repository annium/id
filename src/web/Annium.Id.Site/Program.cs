using System.Threading.Tasks;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Annium.Id.Site
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.ConfigureContainer(new ServiceProviderFactory(x => x.UseServicePack<ServicePack>()));
            builder.Services.AddRuntimeTools(typeof(Program).Assembly);
            await builder.Services.AddConfigurationAsync<Shared.Configuration>(cfg => cfg.AddRemoteYaml($"{builder.HostEnvironment.BaseAddress}site.yml"));
            // builder.Logging.ConfigureLoggingBridge();
            await builder.Build().RunAsync();
        }
    }
}