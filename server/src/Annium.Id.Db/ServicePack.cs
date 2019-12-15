using System;
using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Db
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
                .AddYamlFile(Path.Combine("configuration", "db.yml"))
                .Build<Configuration>();
            services.AddSingleton(cfg);
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            var cfg = provider.GetRequiredService<Configuration>();

            // register context
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<Context>(builder =>
                {
                    builder.UseNpgsql(
                        string.Join(';', new string[]
                        {
                            $"Host={cfg.Host}",
                            $"Port={cfg.Port}",
                            $"Database={cfg.Name}",
                            $"Username={cfg.User}",
                            $"Password={cfg.Password}",
                            $"SSL Mode=Prefer",
                            $"Trust Server Certificate=true",
                        })
                    );
                });

            // log queries if needed
            if (cfg.LogQueries)
            {
                // TODO:
            }
        }
    }
}