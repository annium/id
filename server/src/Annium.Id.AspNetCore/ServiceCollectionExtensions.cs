using System;
using Annium.Id.AspNetCore.Tools;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.AspNetCore
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddIdAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationModelProvider, AuthorizationApplicationModelProvider>();
            services.AddSingleton<Func<AuthorizeIdAttribute, AuthorizationFilter>>(
                sp => attr => new AuthorizationFilter(sp, attr)
            );
            services.AddSingleton<ITokenAccessor, TokenAccessor>();
            services.AddSingleton<ITokenParser, TokenParser>();

            return services;
        }
    }
}