using Annium.Id.AspNetCore.Tools;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NodaTime;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly AuthorizationFilterOptions options;
        private readonly RequestTokenReader requestTokenReader;
        private readonly ITokenReader tokenReader;

        public AuthorizationFilter(
            AuthorizationFilterOptions options,
            RequestTokenReader requestTokenReader,
            ITokenReader tokenReader
        )
        {
            this.options = options;
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

            var tokenReadOptions = new TokenReadOptions
            {
                ValidateAudience = options.ValidateAudience,
                AllowedExpiration = options.AllowedExpiration,
            };
            var readResult = tokenReader.ReadToken(tokenString, tokenReadOptions);
            if (readResult.Status == TokenReadStatus.BadSource)
                return new BadRequestObjectResult(readResult);
            if (readResult.Status == TokenReadStatus.Failed)
                return new UnauthorizedObjectResult(readResult);

            context.HttpContext.Items[Constants.IdTokenProperty] = readResult.Data;

            return null;
        }
    }
}