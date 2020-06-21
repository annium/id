using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure.Email
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            var cfg = new Configuration
            {
                FromAddress = "support@annium.com",
                FromDisplay = "Annium",
            };
            services.AddSingleton(cfg);
        }
    }
}