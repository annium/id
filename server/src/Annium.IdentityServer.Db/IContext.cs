using LinqToDB;
using LinqToDB.Data;

namespace Annium.IdentityServer.Db
{
    internal interface IContext
    {
        ITable<Entities.App> Apps { get; }

        DataConnection GetDataConnection();
    }
}