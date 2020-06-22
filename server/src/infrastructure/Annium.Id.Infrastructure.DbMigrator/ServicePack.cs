using Annium.Core.DependencyInjection;

namespace Annium.Id.Infrastructure.DbMigrator
{
    internal class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<Db.ServicePack>();
        }
    }
}