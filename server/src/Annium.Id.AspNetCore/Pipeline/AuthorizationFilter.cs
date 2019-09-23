using Annium.Id.AspNetCore.Tools;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly RequestTokenReader tokenReader;
        private readonly ITokenParser tokenParser;

        public AuthorizationFilter(
            RequestTokenReader tokenReader,
            ITokenParser tokenParser
        )
        {
            this.tokenReader = tokenReader;
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
            var(tokenString, readResult) = tokenReader.ReadToken(context.HttpContext.Request);
            if (readResult != null)
                return readResult;

            var parseResult = tokenParser.ParseToken<IdToken>(tokenString);
            if (parseResult.Status == TokenParseStatus.BadSource)
                return new BadRequestObjectResult(parseResult);
            if (parseResult.Status == TokenParseStatus.Failed)
                return new UnauthorizedObjectResult(parseResult);

            context.HttpContext.Items[Constants.IdTokenProperty] = parseResult.Data;

            return null;
        }
    }
}