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
            services.AddConfiguration<Configuration>(
                builder => builder.AddYamlFile(Path.Combine("configuration", "email.yml"))
            );
        }

        public override void Register(IServiceCollection services, System.IServiceProvider provider)
        {
            var config = provider.GetRequiredService<Configuration>();
            services.AddSingleton<Net.Mail.Configuration>(config);
            services.AddEmailService();
        }
    }
}