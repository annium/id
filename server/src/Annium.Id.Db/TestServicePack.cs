using System;
using System.Diagnostics;
using Annium.Extensions.DependencyInjection;
using Annium.Extensions.EntityFrameworkCore;
using LinqToDB.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Db
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            services.AddEntityFrameworkSqliteInMemory<Context>();

            // log queries
            DataConnection.TurnTraceSwitchOn(TraceLevel.Verbose);
            DataConnection.WriteTraceLine = (message, context) => Console.WriteLine($"{context}: {message}");
        }
    }
}