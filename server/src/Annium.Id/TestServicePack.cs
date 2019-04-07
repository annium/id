using Annium.Extensions.DependencyInjection;

namespace Annium.Id
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