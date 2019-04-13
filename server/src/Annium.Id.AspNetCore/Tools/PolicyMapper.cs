using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Arguments = System.Collections.Generic.IDictionary<string, object>;

namespace Annium.Id.AspNetCore.Tools
{
    internal class PolicyMapper
    {
        public void EnsureMappable(Policy policy, ActionModel actionModel)
        {
            foreach (var(name, type) in policy.Parameters.Where(p => p.Value != typeof(IdToken)))
                if (actionModel.Parameters.FirstOrDefault(p => p.ParameterName == name && p.ParameterType == type) == null)
                    throw new ArgumentException(
                        $"Policy {policy.Name}, requested by {actionModel.DisplayName} can't be bound due to missing {type} {name} parameter"
                    );
        }

        public Func<IdToken, Arguments, object[]> CreateMapper(Policy policy)
        {
            var token = Expression.Parameter(typeof(IdToken), "token");
            var args = Expression.Parameter(typeof(Arguments), "args");
            var parameters = new [] { token, args };

            var initializers = policy.Parameters.Select(p => CreateInitializer(token, args, p.Key, p.Value)).ToArray();

            var body = Expression.NewArrayInit(typeof(object), initializers);

            return (Func<IdToken, Arguments, object[]>) Expression.Lambda(body, parameters).Compile();
        }

        private Expression CreateInitializer(
            ParameterExpression token,
            ParameterExpression args,
            string name,
            Type type
        )
        {
            if (type == typeof(IdToken))
                return token;

            return Expression.Property(args, "Item", Expression.Constant(name));
        }
    }
}