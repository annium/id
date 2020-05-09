using System;
using Annium.Id.Core;
using Microsoft.AspNetCore.Http;

namespace Annium.Id.AspNetCore.Tools
{
    internal class HttpContextTokenAccessor : ITokenAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpContextTokenAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public IdToken GetToken()
        {
            if (!httpContextAccessor.HttpContext.Items.TryGetValue(Constants.IdTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            return (IdToken) raw;
        }
    }
}