using Annium.Core.DependencyInjection;
using Annium.Id.Infrastructure.Email;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Infrastructure
{
    public class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, System.IServiceProvider provider)
        {
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}