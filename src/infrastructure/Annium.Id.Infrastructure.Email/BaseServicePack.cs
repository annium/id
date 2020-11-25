using System;
using Annium.Core.DependencyInjection;

namespace Annium.Id.Infrastructure.Email
{
    public class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            container.Add<IEmailService, EmailService>().Singleton();
        }
    }
}