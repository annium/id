using System;
using System.Linq;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
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

    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration<Configuration>(x => x.AddCommandLineArgs());
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.AddLogging();
    }

    public override void Setup(IServiceProvider provider)
    {
        var ignored = new[] { "ChainBuilder", "PipeHandler" };
        provider.UseLogging(route => route
            // .UseConsole());
            .For(m => !ignored.Any(m.SubjectType.Contains)).UseConsole());
        SetupNetTypes(provider.Resolve<IMapperConfig>());
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