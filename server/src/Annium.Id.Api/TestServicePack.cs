using System;
using System.IO;
using System.Linq;
using Annium.Core.DependencyInjection;
using Annium.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Api
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
            Add<Db.TestServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            services.AddSingleton(new Application.Configuration
            {
                PrivateKeyFile = Path.Combine("keys", "private.key"),
                PublicKeyFile = Path.Combine("keys", "public.key"),
            });
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            var ignored = new[] { "ChainBuilder", "PipeHandler" };
            services.AddLogging(route => route
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