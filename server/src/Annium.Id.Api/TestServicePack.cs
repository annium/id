using Annium.Extensions.DependencyInjection;

namespace Annium.Id.Api
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