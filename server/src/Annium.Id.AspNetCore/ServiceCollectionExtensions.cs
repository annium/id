using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Annium.Id.AspNetCore;
using Annium.Id.AspNetCore.Tools;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Extensions.DependencyInjection
{
    public static class AuthorizationExtensions
    {
        private static readonly ConditionalWeakTable<IServiceCollection, List<string>> policiesContainer =
            new ConditionalWeakTable<IServiceCollection, List<string>>();

        public static IServiceCollection AddIdAuthorization(this IServiceCollection services)
        {
            // provider
            services.AddSingleton<IApplicationModelProvider, AuthorizationApplicationModelProvider>();

            // filters
            services.AddSingleton<AuthorizationFilter>();
            services.AddSingleton<Func<Policy, PolicyFilter>>(
                sp => policy => new PolicyFilter(policy, sp.GetRequiredService<PolicyMapper>().CreateMapper(policy))
            );

            //tools
            services.AddSingleton<PolicyMapper>();
            services.AddSingleton<TokenAccessor>();
            services.AddSingleton<TokenParser>();

            return services;
        }

        public static IServiceCollection AddIdPolicy<T>(this IServiceCollection services, string name, Expression<Func<T, bool>> expression) =>
            AddPolicy(services, name, expression);

        public static IServiceCollection AddIdPolicy<T1, T2>(this IServiceCollection services, string name, Expression<Func<T1, T2, bool>> expression) =>
            AddPolicy(services, name, expression);

        public static IServiceCollection AddIdPolicy<T1, T2, T3>(this IServiceCollection services, string name, Expression<Func<T1, T2, T3, bool>> expression) =>
            AddPolicy(services, name, expression);

        public static IServiceCollection AddIdPolicy<T1, T2, T3, T4>(this IServiceCollection services, string name, Expression<Func<T1, T2, T3, T4, bool>> expression) =>
            AddPolicy(services, name, expression);

        public static IServiceCollection AddIdPolicy<T1, T2, T3, T4, T5>(this IServiceCollection services, string name, Expression<Func<T1, T2, T3, T4, T5, bool>> expression) =>
            AddPolicy(services, name, expression);

        private static IServiceCollection AddPolicy(
            this IServiceCollection services,
            string name,
            LambdaExpression expression
        )
        {
            var policies = policiesContainer.GetOrCreateValue(services);

            if (policies.Contains(name))
                throw new ArgumentException($"Policy {name} is already registered");

            var parameters = expression.Parameters.ToDictionary(p => p.Name, p => p.Type);
            var handle = expression.Compile();

            var policy = new Policy(name, parameters, handle);
            policies.Add(policy.Name);
            services.AddSingleton(policy);

            return services;
        }
    }
}