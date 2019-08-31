using System;
using Annium.Core.DependencyInjection;
using Annium.Id.Application.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Application
{
    public class ServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            // tools
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
        }
    }
}