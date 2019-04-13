using Annium.Id.AspNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore
{
    internal class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly TokenAccessor tokenAccessor;

        private readonly TokenParser tokenParser;

        public AuthorizationFilter(
            TokenAccessor tokenAccessor,
            TokenParser tokenParser
        )
        {
            this.tokenAccessor = tokenAccessor;
            this.tokenParser = tokenParser;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = HandleAuthorization(context);
            if (result != null)
                context.Result = result;
        }

        private IActionResult HandleAuthorization(AuthorizationFilterContext context)
        {
            var(tokenString, readResult) = tokenAccessor.GetToken(context.HttpContext.Request);
            if (readResult != null)
                return readResult;

            var(token, parseResult) = tokenParser.ParseToken(tokenString);
            if (parseResult != null)
                return parseResult;

            context.ActionDescriptor.Properties[Constants.IdTokenProperty] = token;

            return null;
        }
    }
}