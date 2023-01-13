using Annium.Core.DependencyInjection;

namespace Server.Db.Migrator;

internal class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
        Add<Db.ServicePack>();
    }
}