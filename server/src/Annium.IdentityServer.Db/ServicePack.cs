using System;
using System.Diagnostics;
using System.IO;
using Annium.Extensions.Configuration;
using Annium.Extensions.DependencyInjection;
using LinqToDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.IdentityServer.Db
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
                .AddJsonFile(Path.Combine("configuration", "db.json"))
                .Build<Configuration>();

            // register context
            services
                .AddEntityFrameworkNpgsqlNodaTime()
                .AddDbContext<Context>(builder =>
                {
                    builder.UseNpgsql(string.Join(';', new string[]
                    {
                        $"Host={cfg.Host}",
                        $"Port={cfg.Port}",
                        $"Database={cfg.Name}",
                        $"Username={cfg.User}",
                        $"Password={cfg.Password}",
                    }));
                });

            // log queries if needed
            if (cfg.LogQueries)
            {
                DataConnection.TurnTraceSwitchOn(TraceLevel.Verbose);
                DataConnection.WriteTraceLine = (message, context) => Console.WriteLine($"{context}: {message}");
            }
        }
    }
}