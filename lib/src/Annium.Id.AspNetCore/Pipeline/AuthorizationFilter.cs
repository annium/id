using Annium.Id.AspNetCore.Tools;
using Annium.Id.Core;
using Annium.Identity.Tokens;
using Annium.Identity.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.AspNetCore.Pipeline;

internal class AuthorizationFilter : IAuthorizationFilter
{
    private readonly AuthorizationFilterOptions _options;
    private readonly RequestTokenReader _requestTokenReader;
    private readonly ITokenReader _tokenReader;

    public AuthorizationFilter(
        AuthorizationFilterOptions options,
        RequestTokenReader requestTokenReader,
        ITokenReader tokenReader
    )
    {
        _options = options;
        _requestTokenReader = requestTokenReader;
        _tokenReader = tokenReader;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var result = HandleAuthorization(context);
        if (result != null)
            context.Result = result;
    }

    private IActionResult? HandleAuthorization(AuthorizationFilterContext context)
    {
        var (tokenString, requestReadResult) = _requestTokenReader.ReadToken(context.HttpContext.Request);
        if (requestReadResult != null)
            return requestReadResult;

        var tokenReadOptions = new TokenReadOptions
        {
            ValidateAudience = _options.ValidateAudience,
            ValidateExpiration = _options.ValidateExpiration,
        };
        var readResult = _tokenReader.ReadToken(tokenString, tokenReadOptions);
        // Malformed is the one that is the caller's mistake rather than a rejected credential: the string
        // is not a JWT at all, which is a bad request. Everything else is a token we read and refused
        if (readResult.Status is TokenReadStatus.Malformed)
            return new BadRequestObjectResult(readResult);
        if (readResult.Status is not TokenReadStatus.Ok)
            return new UnauthorizedObjectResult(readResult);

        context.HttpContext.Items[Constants.IdTokenProperty] = readResult.Data;

        return null;
    }
}
