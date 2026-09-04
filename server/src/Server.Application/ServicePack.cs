using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Annium.Configuration.Abstractions;
using Annium.Configuration.Yaml;
using Annium.Core.DependencyInjection;

namespace Server.Application;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
    }

    public override async Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        await container.AddConfigurationAsync<Configuration>(
            builder => builder.AddYamlFile(Path.Combine("configuration", "application.yml")),
            ct
        );
    }
}
