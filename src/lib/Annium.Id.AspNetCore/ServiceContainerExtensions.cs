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
    public static class ServiceContainerExtensions
    {
        private static readonly ConditionalWeakTable<IServiceContainer, List<string>> PoliciesContainer = new();

        public static IServiceContainer AddIdAuthorization(
            this IServiceContainer container,
            Action<IServiceProvider, AuthOptions> configure
        )
        {
            // configure authorization options
            container.AddIdAuthorizationCoreServices(configure);

            // provider
            container.Add<IApplicationModelProvider, AuthorizationApplicationModelProvider>().Singleton();

            // filters
            container.Add<Func<AuthorizationFilterOptions, AuthorizationFilter>>(sp => options => new AuthorizationFilter(
                options,
                sp.Resolve<RequestTokenReader>(),
                sp.Resolve<ITokenReader>()
            )).AsSelf().Singleton();
            container.Add<Func<Policy, PolicyFilter>>(sp => policy => new PolicyFilter(
                sp.Resolve<ITokenAccessor>(),
                policy,
                sp.Resolve<IPolicyMapper>().CreateMapper(policy)
            )).AsSelf().Singleton();

            // tools
            container.Collection.AddHttpContextAccessor();
            container.Add<ITokenAccessor, HttpContextTokenAccessor>().Singleton();
            container.Add<RequestTokenReader>().AsSelf().Singleton();


            return container;
        }

        public static IServiceContainer AddIdPolicy(this IServiceContainer container, string name, Expression<Func<IdToken, bool>> expression)
        {
            return AddPolicy(container, name, expression);
        }

        public static IServiceContainer AddIdPolicy<T>(this IServiceContainer container, string name, Expression<Func<IdToken, T, bool>> expression)
        {
            return AddPolicy(container, name, expression);
        }

        public static IServiceContainer AddIdPolicy<T1, T2>(this IServiceContainer container, string name, Expression<Func<IdToken, T1, T2, bool>> expression)
        {
            return AddPolicy(container, name, expression);
        }

        public static IServiceContainer AddIdPolicy<T1, T2, T3>(this IServiceContainer container, string name,
            Expression<Func<IdToken, T1, T2, T3, bool>> expression)
        {
            return AddPolicy(container, name, expression);
        }

        public static IServiceContainer AddIdPolicy<T1, T2, T3, T4>(this IServiceContainer container, string name,
            Expression<Func<IdToken, T1, T2, T3, T4, bool>> expression)
        {
            return AddPolicy(container, name, expression);
        }

        public static IServiceContainer AddIdPolicy<T1, T2, T3, T4, T5>(this IServiceContainer container, string name,
            Expression<Func<IdToken, T1, T2, T3, T4, T5, bool>> expression)
        {
            return AddPolicy(container, name, expression);
        }

        private static IServiceContainer AddPolicy(
            this IServiceContainer container,
            string name,
            LambdaExpression expression
        )
        {
            var policies = PoliciesContainer.GetOrCreateValue(container);

            if (policies.Contains(name))
                throw new ArgumentException($"Policy {name} is already registered");

            var parameters = expression.Parameters
                .Where(p => !typeof(IdToken).IsAssignableFrom(p.Type))
                .ToDictionary(p => p.Name!, p => p.Type);
            var handle = expression.Compile();

            var policy = new Policy(name, parameters, handle);
            policies.Add(policy.Name);
            container.Add(policy).AsSelf().Singleton();

            return container;
        }
    }
}