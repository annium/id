using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure.Email
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
                .AddYamlFile(Path.Combine("configuration", "email.yml"))
                .Build<Configuration>();
            services.AddSingleton(cfg);
            services.AddSingleton<Net.Mail.Configuration>(cfg);
        }

        public override void Register(IServiceCollection services, System.IServiceProvider provider)
        {
            services.AddEmailService();
        }
    }
}