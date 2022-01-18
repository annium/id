using System;
using Annium.Core.DependencyInjection;
using Annium.Logging.Abstractions;

namespace Annium.Id.Api
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
            Add<Application.TestServicePack>();
            Add<Infrastructure.Db.TestServicePack>();
            Add<Infrastructure.Email.TestServicePack>();
        }

        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            container.AddLogging();
        }

        public override void Setup(IServiceProvider provider)
        {
            var ignored = new[] { "ChainBuilder", "PipeHandler" };
            provider.UseLogging(route => route
                // .For(m =>
                //     !ignored.Any(m.Source.Name.Contains)
                // )
                .For(m =>
                    m.Level >= LogLevel.Warn ||
                    m.Exception != null ||
                    m.Message.Contains("failure", StringComparison.InvariantCultureIgnoreCase)
                )
                .UseConsole());
        }
    }
}