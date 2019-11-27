using System.IO;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Application
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            services.AddSingleton(new Configuration
            {
                PrivateKeyFile = Path.Combine("keys", "private.key"),
                PublicKeyFile = Path.Combine("keys", "public.key"),
            });
        }
    }
}