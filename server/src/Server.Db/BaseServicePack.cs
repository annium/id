using System;
using Annium.Core.DependencyInjection;
using Annium.linq2db.PostgreSql;
using Server.Db.Internal;
using Xdb.Core.Migrations;

namespace Server.Db;

internal class BaseServicePack : ServicePackBase
{
    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.AddPostgreSql<ServerConnection>();

        // repositories
        container.AddAll(GetType().Assembly)
            .Where(x => x.IsClass && x.Name.EndsWith("Repository"))
            .AsInterfaces()
            .Scoped();
    }

    public override void Setup(IServiceProvider provider)
    {
        Migrator.ForPostgresql(provider.Resolve<PostgreSqlConfiguration>().ConnectionString, Constants.Schema)
            .WithScriptsFromAssembly(GetType().Assembly)
            .Execute();
    }
}