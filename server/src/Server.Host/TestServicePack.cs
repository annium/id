using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Annium.Logging;
using Annium.Logging.Console;
using Annium.Logging.Shared;

namespace Server.Host;

public class TestServicePack : ServicePackBase
{
    public TestServicePack()
    {
        Add<BaseServicePack>();
        Add<Application.TestServicePack>();
        Add<Db.TestServicePack>();
        Add<Email.TestServicePack>();
    }

    public override Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        container.AddConfiguration(new Configuration());

        return Task.CompletedTask;
    }

    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
    {
        container.AddLogging();

        return Task.CompletedTask;
    }

    public override Task SetupAsync(IServiceProvider provider, CancellationToken ct)
    {
        var ignored = new[] { "ChainBuilder", "PipeHandler" };
        provider.UseLogging(route =>
            route
                // .For(m =>
                //     !ignored.Any(m.Source.Name.Contains)
                // )
                .For(m =>
                    m.Level >= LogLevel.Warn
                    || m.Exception != null
                    || m.Message.Contains("failure", StringComparison.InvariantCultureIgnoreCase)
                )
                .UseConsole()
        );

        return Task.CompletedTask;
    }
}
