using Annium.Id.AspNetCore.Tools;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly RequestTokenReader requestTokenReader;
        private readonly ITokenReader tokenReader;

        public AuthorizationFilter(
            RequestTokenReader requestTokenReader,
            ITokenReader tokenReader
        )
        {
            this.requestTokenReader = requestTokenReader;
            this.tokenReader = tokenReader;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = HandleAuthorization(context);
            if (result != null)
                context.Result = result;
        }

        private IActionResult? HandleAuthorization(AuthorizationFilterContext context)
        {
            var (tokenString, requestReadResult) = requestTokenReader.ReadToken(context.HttpContext.Request);
            if (requestReadResult != null)
                return requestReadResult;

            var readResult = tokenReader.ReadToken(tokenString);
            if (readResult.Status == TokenReadStatus.BadSource)
                return new BadRequestObjectResult(readResult);
            if (readResult.Status == TokenReadStatus.Failed)
                return new UnauthorizedObjectResult(readResult);

            context.HttpContext.Items[Constants.IdTokenProperty] = readResult.Data;

            return null;
        }
    }
}