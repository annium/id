using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Annium.Architecture.Base;
using Annium.Data.Operations;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class PolicyFilter : IActionFilter
    {
        private readonly ITokenAccessor tokenAccessor;
        private readonly Policy policy;
        private readonly Func<IdToken, IReadOnlyDictionary<string, object>, object[]> mapArguments;

        public PolicyFilter(
            ITokenAccessor tokenAccessor,
            Policy policy,
            Func<IdToken, IReadOnlyDictionary<string, object>, object[]> mapArguments
        )
        {
            this.tokenAccessor = tokenAccessor;
            this.policy = policy;
            this.mapArguments = mapArguments;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = GetToken();
            if (token is null)
            {
                context.Result = GetFailure("No access token");
                return;
            }

            var args = context.ActionArguments.ToDictionary(p => p.Key, p => p.Value);
            var arguments = mapArguments(token, args);

            try
            {
                var result = (bool) policy.Handle.DynamicInvoke(arguments);
                if (!result)
                    context.Result = GetFailure("Access policy violation");
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private IdToken GetToken()
        {
            try
            {
                return tokenAccessor.GetToken();
            }
            catch
            {
                return null;
            }
        }

        private IActionResult GetFailure(string error) =>
            new ObjectResult(Result.New(OperationStatus.Forbidden).Error(error)) { StatusCode = (int) HttpStatusCode.Forbidden };
    }
}