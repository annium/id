using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure.DbMigrator
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Configure(IServiceContainer container)
        {
            container.AddRuntimeTools(GetType().Assembly, false);
            container.AddMapper();
        }
    }
}