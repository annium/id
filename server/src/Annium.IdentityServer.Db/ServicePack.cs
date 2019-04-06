using System;
using System.Diagnostics;
using System.IO;
using Annium.Extensions.Configuration;
using Annium.Extensions.DependencyInjection;
using AutoMapper.Configuration;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.IdentityServer.Db
{
    public class ServicePack : ServicePackBase
    {
        public override void Configure(IServiceCollection services)
        {
            var cfg = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine("configuration", "db.json"))
                .Build<Configuration>();

            // register context itself
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<Context>(builder =>
                {
                    builder.UseNpgsql(string.Join(';', new string[]
                    {
                        $"Host={cfg.Host}",
                        $"Port={cfg.Port}",
                        $"Database={cfg.Name}",
                        $"Username={cfg.User}",
                        $"Password={cfg.Password}",
                    }), options => options.UseNodaTime()); // is needed, cause not enabled by default
                });

            // init linq2db for EF Core
            LinqToDBForEFTools.Initialize();
            if (cfg.LogQueries)
            {
                DataConnection.TurnTraceSwitchOn(TraceLevel.Verbose);
                DataConnection.WriteTraceLine = (message, context) => Console.WriteLine($"{context}: {message}");
            }
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

            services.AddSingleton<MapperConfigurationExpression>(ConfigureMapping());
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddScoped<IContext>(p => p.GetRequiredService<Context>());

            // repositories
            services.AddScoped<IAppRepository, AppRepository>();
        }

        private MapperConfigurationExpression ConfigureMapping()
        {
            var cfg = new MapperConfigurationExpression();

            cfg.CreateMap<App, Entities.App>().ReverseMap();

            return cfg;
        }
    }
}