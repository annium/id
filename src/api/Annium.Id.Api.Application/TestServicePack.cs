using System.IO;
using Annium.Core.DependencyInjection;

namespace Annium.Id.Api.Application
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceContainer container)
        {
            container.Add(new Configuration
            {
                PrivateKeyFile = Path.Combine("keys", "private.key"),
                PublicKeyFile = Path.Combine("keys", "public.key")
            }).AsSelf().Singleton();
        }
    }
}