using System;
using Annium.Core.DependencyInjection;
using Annium.Id.Db.Repositories;
using Annium.Id.Db.Repositories.Implementations;
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
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddScoped<Entities.IContext>(p => p.GetRequiredService<Entities.Context>());

            // repositories
            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<ICompanyClaimRepository, CompanyClaimRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyRoleClaimRepository, CompanyRoleClaimRepository>();
            services.AddScoped<ICompanyRoleRepository, CompanyRoleRepository>();
            services.AddScoped<ICompanyUserClaimRepository, CompanyUserClaimRepository>();
            services.AddScoped<ICompanyUserRepository, CompanyUserRepository>();
            services.AddScoped<ICompanyUserRoleRepository, CompanyUserRoleRepository>();
            services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserAppLoginRepository, UserAppLoginRepository>();
            services.AddScoped<IUserClaimRepository, UserClaimRepository>();
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        }
    }
}