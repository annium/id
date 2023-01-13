using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;

namespace Server.Application;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
    }

    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration<Configuration>(
            builder => builder.AddYamlFile(Path.Combine("configuration", "application.yml"))
        );
    }
}