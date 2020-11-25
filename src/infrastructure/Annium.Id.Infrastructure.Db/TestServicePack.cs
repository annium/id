using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure.Db
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Register(IServiceContainer container, System.IServiceProvider provider)
        {
            container.AddEntityFrameworkSqliteInMemory<Context>();
        }
    }
}