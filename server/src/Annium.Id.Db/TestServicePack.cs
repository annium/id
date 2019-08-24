using Annium.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Db
{
    public class TestServicePack : ServicePackBase
    {
        public TestServicePack()
        {
            Add<BaseServicePack>();
        }

        public override void Register(IServiceCollection services, System.IServiceProvider provider)
        {
            services.AddEntityFrameworkSqliteInMemory<Entities.Context>();

            // log queries
            // LinqToDB.Data.DataConnection.TurnTraceSwitchOn(System.Diagnostics.TraceLevel.Verbose);
            // LinqToDB.Data.DataConnection.WriteTraceLine = (message, context) => System.Console.WriteLine($"{context}: {message}");
        }
    }
}