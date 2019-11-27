using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure
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
                Email = new Email.Configuration
                {
                    FromAddress = "support@annium.com",
                    FromDisplay = "Annium",
                },
            };
            services.AddSingleton(cfg);
            services.AddSingleton(cfg.Email);
        }
    }
}