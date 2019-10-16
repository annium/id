using Annium.Id.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdAuthorizationCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IPolicyMapper, PolicyMapper>();
            services.AddSingleton<ITokenReader, TokenReader>();

            return services;
        }
    }
}