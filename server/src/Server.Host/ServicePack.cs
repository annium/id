using System;
using System.Linq;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;

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
            .For(m => !ignored.Any(m.Source.Contains)).UseConsole());
    }
}