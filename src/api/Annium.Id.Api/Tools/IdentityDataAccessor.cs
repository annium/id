using System;
using System.Linq;
using System.Net;
using Annium.Id.Api.Application.Tools;
using Annium.Id.Domain.Entities.Utility;
using Microsoft.AspNetCore.Http;

namespace Annium.Id.Api.Tools;

internal class IdentityDataAccessor : IIdentityDataAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityDataAccessor(
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IdentityData GetIdentityData()
    {
        var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");

        var ipAddress = context.Connection.RemoteIpAddress ?? IPAddress.Loopback;
        var client = context.Request.Headers.ContainsKey("User-Agent") ? context.Request.Headers["User-Agent"].First() ?? string.Empty : string.Empty;

        return new IdentityData(ipAddress, client);
    }
}