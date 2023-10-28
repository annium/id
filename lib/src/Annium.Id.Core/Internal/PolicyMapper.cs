using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Annium.Id.Core.Internal;

internal class PolicyMapper : IPolicyMapper
{
    public void EnsureMappable(Policy policy, string endpoint, IReadOnlyDictionary<string, Type> parameters)
    {
        foreach (var (name, type) in policy.Parameters)
            if (!parameters.Any(p => p.Key == name && p.Value == type))
                throw new ArgumentException(
                    $"Policy {policy.Name}, requested by {endpoint} can't be bound due to missing {type} {name} parameter"
                );
    }

    public Func<IdToken, IReadOnlyDictionary<string, object>, object[]> CreateMapper(Policy policy)
    {
        var token = Expression.Parameter(typeof(IdToken), "token");
        var args = Expression.Parameter(typeof(IReadOnlyDictionary<string, object>), "args");
        var parameters = new[] { token, args };

        var initializers = new Expression[] { token }
            .Concat(policy.Parameters.Select(p => Expression.Property(args, "Item", Expression.Constant(p.Key))))
            .ToArray();

        var body = Expression.NewArrayInit(typeof(object), initializers);

        return (Func<IdToken, IReadOnlyDictionary<string, object>, object[]>)
            Expression.Lambda(body, parameters).Compile();
    }
}
