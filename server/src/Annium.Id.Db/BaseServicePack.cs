using System;
using Annium.Extensions.DependencyInjection;
using Annium.Extensions.Mapper;
using Annium.Logging.Abstractions;
using AutoMapper.Configuration;
using LinqToDB.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Db
{
    public class BaseServicePack : ServicePackBase
    {
        public override void Configure(IServiceCollection services)
        {
            // init linq2db for EF Core
            LinqToDBForEFTools.Initialize();
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

            services.AddMapperConfiguration(ConfigureMapping);
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddScoped<Entities.IContext>(p => p.GetRequiredService<Entities.Context>());

            // repositories
            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddConsole(new LoggerConfiguration(LogLevel.Trace));
        }

        private MapperConfigurationExpression ConfigureMapping()
        {
            var cfg = new MapperConfigurationExpression();

            cfg.CreateMap<App, Entities.App>().ReverseMap();
            cfg.CreateMap<Instant, DateTime>().ConvertUsing(i => i.ToDateTimeUtc());
            cfg.CreateMap<DateTime, Instant>().ConvertUsing(d => Instant.FromDateTimeUtc(d.ToUniversalTime()));

            return cfg;
        }
    }
}