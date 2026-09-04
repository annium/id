using System.Threading;
using System.Threading.Tasks;
using Annium.Core.DependencyInjection;

namespace Server.Email;

public class TestServicePack : ServicePackBase
{
    public TestServicePack()
    {
        Add<BaseServicePack>();
    }

    public override Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        var cfg = new Configuration { FromAddress = "support@annium.com", FromDisplay = "Annium" };
        container.Add(cfg).AsSelf().Singleton();

        return Task.CompletedTask;
    }
}
