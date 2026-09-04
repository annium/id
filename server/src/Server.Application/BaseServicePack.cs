using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Core.DependencyInjection;
using Annium.Core.Runtime;
using Server.Application.Tools;

namespace Server.Application;

internal class BaseServicePack : ServicePackBase
{
    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
    {
        // tools
        container.Add<ISecurityManager, SecurityManager>().Singleton();
        container.Add<ITokenGenerator, TokenGenerator>().Scoped();

        // services
        container
            .AddAll(GetType().Assembly)
            .Where(x => x.IsClass && x.Name.EndsWith("Service"))
            .AsInterfaces()
            .Scoped();

        return Task.CompletedTask;
    }
}
