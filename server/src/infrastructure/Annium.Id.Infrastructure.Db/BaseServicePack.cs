using System;
using System.Runtime.CompilerServices;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Annium.Id.Infrastructure.DbMigrator")]

namespace Annium.Id.Infrastructure.Db
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddScoped<IContext>(p => p.GetRequiredService<Context>());

            // repositories
            services.AddAssemblyTypes(GetType().Assembly)
                .Where(x => x.IsClass && x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerScope();
        }
    }
}