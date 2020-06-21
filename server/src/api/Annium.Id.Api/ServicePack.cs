using System;
using System.Linq;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Api
{
    public class ServicePack : ServicePackBase
    {
        public ServicePack()
        {
            Add<BaseServicePack>();
            Add<Application.ServicePack>();
            Add<Db.ServicePack>();
            Add<Infrastructure.ServicePack>();
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            var ignored = new[] { "ChainBuilder", "PipeHandler" };
            services.AddLogging(route => route
                // .UseConsole());
                .For(m => !ignored.Any(m.Source.Name.Contains)).UseConsole());
        }
    }
}