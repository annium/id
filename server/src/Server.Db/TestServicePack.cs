using System;
using Annium.Core.DependencyInjection;

namespace Server.Db;

public class TestServicePack : ServicePackBase
{
    public TestServicePack()
    {
        Add<BaseServicePack>();
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        // container.AddEntityFrameworkSqliteInMemory<Context>();
    }
}