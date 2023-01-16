using Annium.Core.DependencyInjection;

namespace Server.Db;

public class TestServicePack : ServicePackBase
{
    public TestServicePack()
    {
        Add<BaseServicePack>();
    }
}