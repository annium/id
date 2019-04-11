using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.AppAuth
{
    internal interface ITokenAccessor
    {
        ValueTuple<Guid, IActionResult> GetToken(HttpRequest request);
    }
}