using Annium.Core.DependencyInjection;

namespace Annium.Id.Infrastructure.DbMigrator
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Configure(IServiceContainer container)
        {
            container.AddRuntime(GetType().Assembly);
            container.AddMapper();
        }
    }
}