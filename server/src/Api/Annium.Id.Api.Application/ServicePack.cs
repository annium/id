using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Application
{
    public class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            var cfg = new ConfigurationBuilder()
                .AddYamlFile(Path.Combine("configuration", "application.yml"))
                .Build<Configuration>();
            services.AddSingleton(cfg);
        }
    }
}