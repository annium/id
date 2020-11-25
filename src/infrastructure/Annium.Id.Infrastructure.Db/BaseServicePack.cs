using System;
using System.Runtime.CompilerServices;
using Annium.Core.DependencyInjection;

[assembly: InternalsVisibleTo("Annium.Id.Infrastructure.DbMigrator")]

namespace Annium.Id.Infrastructure.Db
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            container.Add<IContext, Context>().Scoped();

            // repositories
            container.AddAll(GetType().Assembly)
                .Where(x => x.IsClass && x.Name.EndsWith("Repository"))
                .AsInterfaces()
                .Scoped();
        }
    }
}