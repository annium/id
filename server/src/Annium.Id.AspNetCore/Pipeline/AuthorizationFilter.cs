using Annium.Id.AspNetCore.Tools;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly RequestTokenReader tokenAccessor;
        private readonly ITokenParser tokenParser;

        public AuthorizationFilter(
            RequestTokenReader tokenAccessor,
            ITokenParser tokenParser
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
            var(tokenString, readResult) = tokenAccessor.ReadToken(context.HttpContext.Request);
            if (readResult != null)
                return readResult;

            var parseResult = tokenParser.ParseToken(tokenString);
            if (parseResult.Status == TokenParseStatus.BadSource)
                return new BadRequestObjectResult(parseResult);
            if (parseResult.Status == TokenParseStatus.Failed)
                return new UnauthorizedObjectResult(parseResult);

            var token = parseResult.Data;

            context.HttpContext.Items[Constants.IdBaseTokenProperty] = token;
            if (token is IdAppToken)
                context.HttpContext.Items[Constants.IdAppTokenProperty] = token;

            return null;
        }
    }
}