using System;
using System.Linq;
using System.Linq.Expressions;
using Annium.Id.AspNetCore.Pipeline;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Arguments = System.Collections.Generic.IDictionary<string, object>;

namespace Annium.Id.AspNetCore.Tools
{
    internal class PolicyMapper
    {
        public void EnsureMappable(Policy policy, ActionModel actionModel)
        {
            foreach (var(name, type) in policy.Parameters)
                if (actionModel.Parameters.FirstOrDefault(p => p.ParameterName == name && p.ParameterType == type) == null)
                    throw new ArgumentException(
                        $"Policy {policy.Name}, requested by {actionModel.DisplayName} can't be bound due to missing {type} {name} parameter"
                    );
        }

        public Func<IdAppToken, Arguments, object[]> CreateMapper(Policy policy)
        {
            var token = Expression.Parameter(typeof(IdAppToken), "token");
            var args = Expression.Parameter(typeof(Arguments), "args");
            var parameters = new [] { token, args };

            var initializers = new Expression[] { token }
                .Concat(policy.Parameters.Select(p => Expression.Property(args, "Item", Expression.Constant(p.Key))))
                .ToArray();

            var body = Expression.NewArrayInit(typeof(object), initializers);

            return (Func<IdAppToken, Arguments, object[]>) Expression.Lambda(body, parameters).Compile();
        }
    }
}