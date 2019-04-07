using System;
using System.Diagnostics;
using Annium.Extensions.DependencyInjection;
using LinqToDB.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.IdentityServer.Db
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            // register context itself
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<Context>(builder =>
                {
                    var cn = new SqliteConnection("Data Source=:memory:");
                    cn.Open();
                    var opts = builder.UseSqlite(cn).Options;
                    using(var ctx = new Context(opts)) ctx.Database.EnsureCreated();
                });

            // log queries
            DataConnection.TurnTraceSwitchOn(TraceLevel.Verbose);
            DataConnection.WriteTraceLine = (message, context) => Console.WriteLine($"{context}: {message}");
        }
    }
}