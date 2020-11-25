using Annium.Core.DependencyInjection;

namespace Annium.Id.Infrastructure.Email
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceContainer container)
        {
            var cfg = new Configuration
            {
                FromAddress = "support@annium.com",
                FromDisplay = "Annium"
            };
            container.Add(cfg).AsSelf().Singleton();
        }
    }
}