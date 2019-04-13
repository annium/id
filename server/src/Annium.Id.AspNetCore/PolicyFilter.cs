using System;
using System.Collections.Generic;
using Annium.Data.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore
{
    internal class PolicyFilter : IActionFilter
    {
        private readonly Policy policy;

        private readonly Func<IdToken, IDictionary<string, object>, object[]> mapArguments;

        public PolicyFilter(
            Policy policy,
            Func<IdToken, IDictionary<string, object>, object[]> mapArguments
        )
        {
            this.policy = policy;
            this.mapArguments = mapArguments;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var arguments = mapArguments((IdToken) context.ActionDescriptor.Properties[Constants.IdTokenProperty], context.ActionArguments);

            var result = (bool) policy.Handle.DynamicInvoke(arguments);
            if (!result)
                context.Result = new ObjectResult(Result.Failure().Error("Access policy violation"));
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}