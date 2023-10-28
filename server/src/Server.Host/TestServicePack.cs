using System;
using Annium.Core.DependencyInjection;
using Annium.Logging;

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

    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration(new Configuration());
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.AddLogging();
    }

    public override void Setup(IServiceProvider provider)
    {
        var ignored = new[] { "ChainBuilder", "PipeHandler" };
        provider.UseLogging(
            route =>
                route
                    // .For(m =>
                    //     !ignored.Any(m.Source.Name.Contains)
                    // )
                    .For(
                        m =>
                            m.Level >= LogLevel.Warn
                            || m.Exception != null
                            || m.Message.Contains("failure", StringComparison.InvariantCultureIgnoreCase)
                    )
                    .UseConsole()
        );
    }
}
