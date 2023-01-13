using Annium.Core.DependencyInjection;

namespace Server.Db.Migrator;

internal class BaseServicePack : ServicePackBase
{
    public override void Configure(IServiceContainer container)
    {
        container.AddRuntime(GetType().Assembly);
        container.AddMapper();
    }
}