using System;
using Annium.Extensions.DependencyInjection;
using AutoMapper.Configuration;
using LinqToDB.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.IdentityServer.Db
{
    public class BaseServicePack : ServicePackBase
    {
        public override void Configure(IServiceCollection services)
        {
            // init linq2db for EF Core
            LinqToDBForEFTools.Initialize();
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