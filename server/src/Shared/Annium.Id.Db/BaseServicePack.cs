using System;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Db
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddScoped<IContext>(p => p.GetRequiredService<Context>());

            // repositories
            services.AddAssemblyTypes()
                .Where(x => x.IsClass && x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerScope();
        }
    }
}