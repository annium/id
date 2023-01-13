using System;
using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Db;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
    }

    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration<Configuration>(
            builder => builder.AddYamlFile(Path.Combine("configuration", "db.yml"))
        );
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        // register context
        container.Collection
            .AddDbContext<Context>((sp, builder) =>
            {
                var cfg = sp.Resolve<Configuration>();
                builder.UseNpgsql(
                    string.Join(';', $"Host={cfg.Host}", $"Port={cfg.Port}", $"Database={cfg.Database}", $"Username={cfg.User}", $"Password={cfg.Password}", "SSL Mode=Prefer", "Trust Server Certificate=true"),
                    options =>
                    {
                        options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), Array.Empty<string>());
                        options.MigrationsAssembly("Server.Db.Migrator");
                    }
                );
            });
    }
}