using System;
using System.Threading.Tasks;

namespace Server.Db.Internal.Repositories;

internal abstract class RepositoryBase : IAsyncDisposable
{
    protected readonly ServerConnection Db;
    private bool _isDisposed;

    protected RepositoryBase(ServerConnection db)
    {
        Db = db;
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
            return;
        _isDisposed = true;

        await Db.DisposeAsync();
    }
}