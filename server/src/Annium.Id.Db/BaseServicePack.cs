using System;
using Annium.Extensions.DependencyInjection;
using Annium.Extensions.Mapper;
using AutoMapper.Configuration;
using LinqToDB.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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