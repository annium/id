using LinqToDB;
using LinqToDB.Data;

namespace Annium.Id.Db
{
    internal interface IContext
    {
        ITable<Entities.App> Apps { get; }

        DataConnection GetDataConnection();
    }
}