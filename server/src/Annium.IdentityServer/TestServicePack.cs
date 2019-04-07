using Annium.Extensions.DependencyInjection;

namespace Annium.IdentityServer
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
            Add<Db.TestServicePack>();
        }
    }
}