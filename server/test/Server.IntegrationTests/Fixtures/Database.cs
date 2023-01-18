using System.Threading;
using System.Threading.Tasks;
using Annium.linq2db.PostgreSql;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Server.Db.Internal;
using Xdb.Core.Migrations;

namespace Server.IntegrationTests.Fixtures;

public static class Database
{
    public static PostgreSqlConfiguration Config { get; } = new()
    {
        Database = "db",
        User = "postgres",
        Password = "postgres",
    };

    private static readonly TestcontainerDatabase Db;
    private static readonly TaskCompletionSource InitTcs = new();
    private static volatile int _refs;

    static Database()
    {
        Db = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration("registry.annium.com/postgres:15")
            {
                Database = Config.Database,
                Username = Config.User,
                Password = Config.Password,
            })
            .Build();
    }

    public static async Task AcquireAsync()
    {
        if (Interlocked.Increment(ref _refs) > 1)
        {
            await InitTcs.Task;
            return;
        }

        await Db.StartAsync();
        Config.Host = Db.Hostname;
        Config.Port = Db.Port;
        Migrator.ForPostgresql(Config.ConnectionString, Constants.Schema)
            .WithScriptsFromAssembly(typeof(Db.TestServicePack).Assembly)
            .Execute();
        InitTcs.SetResult();
    }
}