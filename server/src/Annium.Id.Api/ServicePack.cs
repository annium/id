using Annium.Extensions.DependencyInjection;

namespace Annium.Id.Api
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