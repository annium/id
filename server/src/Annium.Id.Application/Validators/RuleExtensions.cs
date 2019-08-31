using System;
using System.Threading.Tasks;
using Annium.Extensions.Validation;

namespace Annium.Id.Application.Validators
{
    internal static class RuleExtensions
    {
        public static IRuleBuilder<TValue, TField> Unique<TValue, TField>(
            this IRuleBuilder<TValue, TField> rule,
            Func<TField, Task<object>> getEntityAsync,
            string message = null
        ) => rule.Add(async(context, value) =>
        {
            var entity = await getEntityAsync(value);
            if (entity != null)
                context.Error(message ?? "{0} with {1} {2} already exists", typeof(TValue).Name, context.Field, value);
        });
    }
}