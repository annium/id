using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Annium.Configuration.Abstractions;
using Annium.Configuration.Yaml;
using Annium.Core.DependencyInjection;
using Annium.Net.Mail;

namespace Server.Email;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
    }

    public override Task ConfigureAsync(IServiceContainer container, CancellationToken ct) =>
        container.AddConfigurationAsync<Configuration>(
            builder => builder.AddYamlFile(Path.Combine("configuration", "email.yml")),
            ct
        );

    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
    {
        container.Add<Annium.Net.Mail.Configuration, Configuration>().Singleton();
        container.AddEmailService();

        return Task.CompletedTask;
    }
}
