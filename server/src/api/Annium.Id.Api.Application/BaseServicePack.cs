using System;
using Annium.Core.DependencyInjection;
using Annium.Id.Api.Application.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Api.Application
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            // tools
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            // services
            services.AddAssemblyTypes()
                .Where(x => x.IsClass && x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerScope();
        }
    }
}