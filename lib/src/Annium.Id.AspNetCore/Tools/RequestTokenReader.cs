using System;
using System.Linq;
using System.Net;
using Annium.Data.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Annium.Id.AspNetCore.Tools;

internal class RequestTokenReader
{
    public ValueTuple<string, IActionResult?> ReadToken(HttpRequest request)
    {
        if (!request.Headers.ContainsKey(HeaderNames.Authorization))
            return Fail(HttpStatusCode.Unauthorized, "Bearer authorization required.");
        var authorization = request.Headers[HeaderNames.Authorization]
            .ToString()
            .Split(' ')
            .Select(e => e.Trim())
            .ToArray();
        if (authorization.Length != 2)
            return Fail(HttpStatusCode.Unauthorized, "Authorization format is invalid.");

        var (type, token) = (authorization[0], authorization[1]);
        if (type != "Bearer")
            return Fail(HttpStatusCode.Unauthorized, "Bearer authorization required.");

        return (token, null);

        static (string, IActionResult) Fail(HttpStatusCode statusCode, string message)
        {
            return (string.Empty, new ObjectResult(Result.Failure().Error(message)) { StatusCode = (int)statusCode });
        }
    }
}
