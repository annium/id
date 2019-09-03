using System.Linq;
using System.Net;
using Annium.Id.Application.Tools;
using Microsoft.AspNetCore.Http;

namespace Annium.Id.Api.Tools
{
    internal class IdentityDataAccessor : IIdentityDataAccessor
    {
        private IHttpContextAccessor httpContextAccessor;

        public IdentityDataAccessor(
            IHttpContextAccessor httpContextAccessor
        )
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public(IPAddress ipAddress, string client) GetIdentityData()
        {
            var context = httpContextAccessor.HttpContext;

            var ipAddress = context.Connection.RemoteIpAddress ?? IPAddress.Loopback;
            var client = context.Request.Headers.ContainsKey("User-Agent") ?
                context.Request.Headers["User-Agent"].First() :
                string.Empty;

            return (ipAddress, client);
        }

    }
}