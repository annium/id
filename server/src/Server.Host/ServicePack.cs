using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Configuration.Abstractions;
using Annium.Configuration.CommandLine;
using Annium.Core.DependencyInjection;
using Annium.Logging.Console;
using Annium.Logging.Shared;
using Annium.Net.Types;

namespace Server.Host;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
        Add<Application.ServicePack>();
        Add<Db.ServicePack>();
        Add<Email.ServicePack>();
    }

    public override async Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        await container.AddConfigurationAsync<Configuration>(x => x.AddCommandLineArgs(), ct);
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
                // .UseConsole());
                .For(m => !ignored.Any(m.SubjectType.Contains))
                .UseConsole()
        );
        SetupNetTypes(provider.Resolve<IMapperConfig>());

        return Task.CompletedTask;
    }

    private void SetupNetTypes(IMapperConfig config)
    {
        config.Exclude(Match.NamespaceStartsWith("Annium.Architecture"));
        config.Exclude(Match.NamespaceStartsWith("Annium.Data"));
        config.Exclude(Match.NamespaceStartsWith("Annium.Logging"));
        config.Exclude(Match.NamespaceStartsWith("Server.Domain"));
        config.Exclude(Match.NamespaceStartsWith("Server.ViewModels"));
    }
}
