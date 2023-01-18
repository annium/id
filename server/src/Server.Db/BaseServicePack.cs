using System;
using Annium.Core.DependencyInjection;

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
}