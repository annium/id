using System;
using Annium.Core.DependencyInjection;
using Server.Application.Tools;

namespace Server.Application;

internal class BaseServicePack : ServicePackBase
{
    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        // tools
        container.Add<ISecurityManager, SecurityManager>().Singleton();
        container.Add<ITokenGenerator, TokenGenerator>().Scoped();

        // services
        container.AddAll(GetType().Assembly)
            .Where(x => x.IsClass && x.Name.EndsWith("Service"))
            .AsInterfaces()
            .Scoped();
    }
}