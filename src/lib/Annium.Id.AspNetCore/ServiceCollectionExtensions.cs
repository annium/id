using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Annium.Id.AspNetCore.Pipeline;
using Annium.Id.AspNetCore.Tools;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Core.DependencyInjection
{
    public static class AuthorizationExtensions
    {
        private static readonly ConditionalWeakTable<IServiceCollection, List<string>> policiesContainer =
            new ConditionalWeakTable<IServiceCollection, List<string>>();

        public static IServiceCollection AddIdAuthorization(
            this IServiceCollection services,
            Action<AuthOptions> configure
        )
        {
            // configure authorization options
            services.AddIdAuthorizationCoreServices(configure);

            // provider
            services.AddSingleton<IApplicationModelProvider, AuthorizationApplicationModelProvider>();

            // filters
            services.AddSingleton<Func<AuthorizationFilterOptions, AuthorizationFilter>>(sp => options => new AuthorizationFilter(
                options,
                sp.GetRequiredService<RequestTokenReader>(),
                sp.GetRequiredService<ITokenReader>()
            ));
            services.AddSingleton<Func<Policy, PolicyFilter>>(sp => policy => new PolicyFilter(
                sp.GetRequiredService<ITokenAccessor>(),
                policy,
                sp.GetRequiredService<IPolicyMapper>().CreateMapper(policy)
            ));

            // tools
            services.AddHttpContextAccessor();
            services.AddSingleton<ITokenAccessor, HttpContextTokenAccessor>();
            services.AddSingleton<RequestTokenReader>();


            return services;
        }

        public static IServiceCollection AddIdPolicy(this IServiceCollection services, string name, Expression<Func<IdToken, bool>> expression)
        {
            return AddPolicy(services, name, expression);
        }

        public static IServiceCollection AddIdPolicy<T>(this IServiceCollection services, string name, Expression<Func<IdToken, T, bool>> expression)
        {
            return AddPolicy(services, name, expression);
        }

        public static IServiceCollection AddIdPolicy<T1, T2>(this IServiceCollection services, string name, Expression<Func<IdToken, T1, T2, bool>> expression)
        {
            return AddPolicy(services, name, expression);
        }

        public static IServiceCollection AddIdPolicy<T1, T2, T3>(this IServiceCollection services, string name,
            Expression<Func<IdToken, T1, T2, T3, bool>> expression)
        {
            return AddPolicy(services, name, expression);
        }

        public static IServiceCollection AddIdPolicy<T1, T2, T3, T4>(this IServiceCollection services, string name,
            Expression<Func<IdToken, T1, T2, T3, T4, bool>> expression)
        {
            return AddPolicy(services, name, expression);
        }

        public static IServiceCollection AddIdPolicy<T1, T2, T3, T4, T5>(this IServiceCollection services, string name,
            Expression<Func<IdToken, T1, T2, T3, T4, T5, bool>> expression)
        {
            return AddPolicy(services, name, expression);
        }

        private static IServiceCollection AddPolicy(
            this IServiceCollection services,
            string name,
            LambdaExpression expression
        )
        {
            var policies = policiesContainer.GetOrCreateValue(services);

            if (policies.Contains(name))
                throw new ArgumentException($"Policy {name} is already registered");

            var parameters = expression.Parameters
                .Where(p => !typeof(IdToken).IsAssignableFrom(p.Type))
                .ToDictionary(p => p.Name, p => p.Type);
            var handle = expression.Compile();

            var policy = new Policy(name, parameters, handle);
            policies.Add(policy.Name);
            services.AddSingleton(policy);

            return services;
        }
    }
}