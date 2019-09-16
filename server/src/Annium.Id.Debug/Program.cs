using System;
using System.Threading;
using Annium.Core.Entrypoint;

namespace Annium.Id.Debug
{
    public class Program
    {
        private static void Run(
            IServiceProvider provider,
            string[] args,
            CancellationToken token
        )
        {
            // var type = typeof(MappingEnumerablePipeHandler<, ,>);
            // var target = typeof(IRequestHandlerInput<ListAppsQuery, IStatusResult<OperationStatus, IEnumerable<AppPublicResponse>>>);
            // var resolution = type.ResolveGenericArgumentsByImplentation(target);
        }

        public static int Main(string[] args) => new Entrypoint()
            .UseServicePack<ServicePack>()
            .Run(Run, args);
    }
}