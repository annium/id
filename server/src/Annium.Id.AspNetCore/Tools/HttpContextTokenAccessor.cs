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

        public IdAppToken GetAppToken()
        {
            if (!httpContextAccessor.HttpContext.Items.TryGetValue(Constants.IdAppTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            return (IdAppToken) raw;
        }

        public IdBaseToken GetBaseToken()
        {
            if (!httpContextAccessor.HttpContext.Items.TryGetValue(Constants.IdBaseTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            return (IdBaseToken) raw;
        }
    }
}