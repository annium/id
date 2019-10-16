using Annium.Id.Core;
using Annium.Id.Core.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdAuthorizationCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IPolicyMapper, PolicyMapper>();
            services.AddSingleton<ITokenReader, TokenReader>();
            services.AddSingleton<ITokenWriter, TokenWriter>();

            return services;
        }
    }
}