using System;
using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure.Db
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
                builder => builder.AddYamlFile(Path.Combine("configuration", "db.yml"))
            );
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            // register context
            services
                .AddDbContext<Context>((sp, builder) =>
                {
                    var cfg = sp.GetRequiredService<Configuration>();
                    builder.UseNpgsql(
                        string.Join(';', new string[]
                        {
                            $"Host={cfg.Host}",
                            $"Port={cfg.Port}",
                            $"Database={cfg.Database}",
                            $"Username={cfg.User}",
                            $"Password={cfg.Password}",
                            $"SSL Mode=Prefer",
                            $"Trust Server Certificate=true"
                        }),
                        options =>
                        {
                            options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), Array.Empty<string>());
                            options.MigrationsAssembly("Annium.Id.Infrastructure.DbMigrator");
                        }
                    );
                });
        }
    }
}