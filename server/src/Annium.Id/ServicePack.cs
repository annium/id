using Annium.Extensions.DependencyInjection;

namespace Annium.Id
{
    public class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<BaseServicePack>();
            Add<Db.ServicePack>();
        }
    }
}