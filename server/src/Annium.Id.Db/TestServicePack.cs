using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Db
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Register(IServiceCollection services, System.IServiceProvider provider)
        {
            services.AddEntityFrameworkSqliteInMemory<Context>();
        }
    }
}