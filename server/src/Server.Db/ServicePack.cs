using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Annium.linq2db.PostgreSql;

namespace Server.Db;

public class ServicePack : ServicePackBase
{
    public ServicePack()
    {
        Add<BaseServicePack>();
    }

    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration<PostgreSqlConfiguration>(x => x
            .AddYamlFile(Path.Combine("configuration", "db.yml"))
        );
    }
}