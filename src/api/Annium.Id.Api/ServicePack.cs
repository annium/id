using System;
using System.Linq;
using Annium.Core.DependencyInjection;

namespace Annium.Id.Api
{
    public class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<BaseServicePack>();
            Add<Application.ServicePack>();
            Add<Infrastructure.Db.ServicePack>();
            Add<Infrastructure.Email.ServicePack>();
        }

        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            var ignored = new[] { "ChainBuilder", "PipeHandler" };
            container.AddLogging(route => route
                // .UseConsole());
                .For(m => !ignored.Any(m.Source.Name.Contains)).UseConsole());
        }
    }
}