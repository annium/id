using System;
using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;

namespace Server.Email;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
    }

    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration<Configuration>(builder =>
            builder.AddYamlFile(Path.Combine("configuration", "email.yml"))
        );
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.Add<Annium.Net.Mail.Configuration, Configuration>().Singleton();
        container.AddEmailService();
    }
}
