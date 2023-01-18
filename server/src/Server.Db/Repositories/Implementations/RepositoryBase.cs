using System;
using System.Threading.Tasks;
using Annium.Core.Primitives;

namespace Server.Db.Repositories.Implementations;

internal abstract class RepositoryBase : IAsyncDisposable
{
    protected readonly ServerConnection Db;

    protected RepositoryBase(ServerConnection db)
    {
        Db = db;
    }

    public async ValueTask DisposeAsync()
    {
        await Db.DisposeAsync();
    }
}