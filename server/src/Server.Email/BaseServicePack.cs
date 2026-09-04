using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Core.DependencyInjection;

namespace Server.Email;

public class BaseServicePack : ServicePackBase
{
    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
    {
        container.Add<IEmailService, EmailService>().Singleton();

        return Task.CompletedTask;
    }
}
