using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.DbUp.Core;
using Annium.DbUp.PostgreSql;
using Server.Db;
using Server.Db.Internal;
using Testcontainers.PostgreSql;
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

    private static readonly PostgreSqlContainer _db;
    private static readonly TaskCompletionSource _initTcs = new();
    private static volatile int _refs;

    static Database()
    {
        _db = new PostgreSqlBuilder("annium/postgres:17-alpine")
            .WithDatabase(Config.Database)
            .WithUsername(Config.User)
            .WithPassword(Config.Password)
            .Build();
        // Db = new ContainerBuilder<PostgreSqlTestcontainer>()
        //     .WithDatabase(new PostgreSqlTestcontainerConfiguration("annium/postgres:17-alpine")
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
#pragma warning disable VSTHRD003
            await _initTcs.Task;
#pragma warning restore VSTHRD003
            return;
        }

        // every other test waits on this one completing, so a failure has to be published rather than
        // left to hang the whole run - the first test then reports the real cause and the rest fail fast
        try
        {
            await _db.StartAsync();
            Config.Host = _db.Hostname;
            Config.Port = _db.GetMappedPublicPort(PostgreSqlBuilder.PostgreSqlPort);
            Migrator
                .Instance.ForPostgresql(Config.ConnectionString, Constants.Schema)
                .WithScriptsFromAssembly(typeof(TestServicePack).Assembly)
                .Execute();
            _initTcs.SetResult();
        }
        catch (Exception e)
        {
            _initTcs.SetException(e);
            throw;
        }
    }
}
