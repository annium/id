using System;
using System.IO;
using System.Linq;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Api
{
    public class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<BaseServicePack>();
            Add<Db.ServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            var cfg = new ConfigurationBuilder()
                .AddYamlFile(Path.Combine("configuration", "api.yml"))
                .Build<Application.Configuration>();
            services.AddSingleton(cfg);
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            var ignored = new[] { "ChainBuilder", "PipeHandler" };
            services.AddLogging(route => route
                // .UseConsole());
                .For(m => !ignored.Any(m.Source.Name.Contains)).UseConsole());
        }
    }
}