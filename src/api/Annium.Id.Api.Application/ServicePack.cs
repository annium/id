using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Api.Application
{
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
}