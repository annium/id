using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Annium.Data.Operations;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class PolicyFilter : IActionFilter
    {
        private readonly Policy policy;

        private readonly Func<IdAppToken, IReadOnlyDictionary<string, object>, object[]> mapArguments;

        public PolicyFilter(
            Policy policy,
            Func<IdAppToken, IReadOnlyDictionary<string, object>, object[]> mapArguments
        )
        {
            this.policy = policy;
            this.mapArguments = mapArguments;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionDescriptor.Properties.ContainsKey(Constants.IdAppTokenProperty))
            {
                // TODO: cleanup
                context.Result = new ObjectResult(Result.Failure().Error("Access policy violation")) { StatusCode = (int) HttpStatusCode.Forbidden };
                return;
            }

            var token = (IdAppToken) context.ActionDescriptor.Properties[Constants.IdAppTokenProperty];
            var args = context.ActionArguments.ToDictionary(p => p.Key, p => p.Value);
            var arguments = mapArguments(token, args);

            try
            {
                var result = (bool) policy.Handle.DynamicInvoke(arguments);
                if (!result)
                    context.Result = new ObjectResult(Result.Failure().Error("Access policy violation")) { StatusCode = (int) HttpStatusCode.Forbidden };
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}