using System;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.AspNetCore.Tools
{
    internal interface ITokenParser
    {
        ValueTuple<IdToken, IActionResult> ParseToken(string tokenString);
    }
}