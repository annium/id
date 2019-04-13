using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.AspNetCore.Tools
{
    internal interface ITokenAccessor
    {
        ValueTuple<string, IActionResult> GetToken(HttpRequest request);
    }
}