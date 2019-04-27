using Annium.Id.Api.AppAuth;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Extensions.DependencyInjection
{
    internal static class AuthorizationExtensions
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationModelProvider, AuthorizationApplicationModelProvider>();
            services.AddSingleton<AuthorizationFilter>();
            services.AddSingleton<ITokenAccessor, BearerTokenAccessor>();

            return services;
        }
    }
}