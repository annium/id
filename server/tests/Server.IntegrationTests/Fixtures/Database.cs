using System.Threading;
using System.Threading.Tasks;
using Server.Db;
using Server.Db.Internal;
using Testcontainers.PostgreSql;
using Xdb.Core.Migrations;
using PostgreSqlConfiguration = Annium.linq2db.PostgreSql.PostgreSqlConfiguration;

namespace Server.IntegrationTests.Fixtures;

public static class Database
{
    public static PostgreSqlConfiguration Config { get; } =
        new()
        {
            Database = "db",
            User = "postgres",
            Password = "postgres",
        };

    private static readonly PostgreSqlContainer Db;
    private static readonly TaskCompletionSource InitTcs = new();
    private static volatile int _refs;

    static Database()
    {
        Db = new PostgreSqlBuilder()
            .WithImage("registry.annium.com/postgres:15")
            .WithDatabase(Config.Database)
            .WithUsername(Config.User)
            .WithPassword(Config.Password)
            .Build();
        // Db = new ContainerBuilder<PostgreSqlTestcontainer>()
        //     .WithDatabase(new PostgreSqlTestcontainerConfiguration("registry.annium.com/postgres:15")
        //     {
        //         Database = Config.Database,
        //         Username = Config.User,
        //         Password = Config.Password,
        //     })
        //     .Build();
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
        Config.Port = Db.GetMappedPublicPort(PostgreSqlBuilder.PostgreSqlPort);
        Migrator
            .ForPostgresql(Config.ConnectionString, Constants.Schema)
            .WithScriptsFromAssembly(typeof(TestServicePack).Assembly)
            .Execute();
        InitTcs.SetResult();
    }
}
