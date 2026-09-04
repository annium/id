using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Annium.Core.DependencyInjection;

namespace Server.Application;

public class TestServicePack : ServicePackBase
{
    public TestServicePack()
    {
        Add<BaseServicePack>();
    }

    public override Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        container
            .Add(
                new Configuration
                {
                    PrivateKeyFile = Path.Combine("keys", "private.key"),
                    PublicKeyFile = Path.Combine("keys", "public.key"),
                }
            )
            .AsSelf()
            .Singleton();

        return Task.CompletedTask;
    }
}
