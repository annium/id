using System;
using Annium.Id.Core;
using Microsoft.AspNetCore.Http;

namespace Annium.Id.AspNetCore.Tools
{
    internal class HttpContextTokenAccessor : ITokenAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IdToken GetToken()
        {
            var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            if (!context.Items.TryGetValue(Constants.IdTokenProperty, out var raw))
                throw new InvalidOperationException("User is not authenticated.");

            return (IdToken) raw!;
        }
    }
}