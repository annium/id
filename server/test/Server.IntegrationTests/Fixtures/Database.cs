using System.Threading;
using System.Threading.Tasks;
using Annium.linq2db.PostgreSql;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

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
            return;

        await Db.StartAsync();
        Config.Host = Db.Hostname;
        Config.Port = Db.Port;
    }
}