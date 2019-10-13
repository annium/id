using Annium.Id.Apps.AspNetCore.Pipeline;
using Annium.Id.Apps.AspNetCore.Tools;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Core.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationModelProvider, AuthorizationApplicationModelProvider>();
            services.AddSingleton<AuthorizationFilter>();
            services.AddSingleton<RequestTokenReader>();

            return services;
        }
    }
}