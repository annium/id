using Annium.EntityFrameworkCore.Extensions;
using Annium.Extensions.DependencyInjection;
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
            services.AddEntityFrameworkSqliteInMemory<Entities.Context>();

            // log queries
            // DataConnection.TurnTraceSwitchOn(TraceLevel.Verbose);
            // DataConnection.WriteTraceLine = (message, context) => Console.WriteLine($"{context}: {message}");
        }
    }
}