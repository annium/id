using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Annium.Data.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class PolicyFilter : IActionFilter
    {
        private readonly Policy policy;

        private readonly Func<IdAppToken, IDictionary<string, object>, object[]> mapArguments;

        public PolicyFilter(
            Policy policy,
            Func<IdAppToken, IDictionary<string, object>, object[]> mapArguments
        )
        {
            this.policy = policy;
            this.mapArguments = mapArguments;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionDescriptor.Properties.ContainsKey(Constants.IdAppTokenProperty))
            {
                context.Result = new ObjectResult(Result.Failure().Error("Access policy violation")) { StatusCode = (int) HttpStatusCode.Forbidden };
                return;
            }

            var arguments = mapArguments((IdAppToken) context.ActionDescriptor.Properties[Constants.IdAppTokenProperty], context.ActionArguments);

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