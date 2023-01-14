using Annium.linq2db.Extensions.Models;
using Annium.Logging.Abstractions;

namespace Server.Db;

public class ServerConnection : DataConnectionBase, ILogSubject<ServerConnection>
{
    public ILogger<ServerConnection> Logger { get; }

    public ServerConnection(
        Config<ServerConnection> config,
        ILogger<ServerConnection> logger
    ) : base(config.Options)
    {
        Logger = logger;
    }
}