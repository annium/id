using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Server.Application.Tools;

namespace Server.Host.Tools;

internal class IdentityDataAccessor : IIdentityDataAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityDataAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IdentityData GetIdentityData()
    {
        var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");

        var ipAddress = context.Connection.RemoteIpAddress ?? IPAddress.Loopback;
        var client = context.Request.Headers.TryGetValue("User-Agent", out var header)
            ? header.First() ?? string.Empty
            : string.Empty;

        return new IdentityData(ipAddress, client);
    }
}
