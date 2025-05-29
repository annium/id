using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Net.Http;
using Server.DemoHost.ViewModels;

namespace Server.DemoHost.TestClient.Clients;

public class IndexClient
{
    private readonly IHttpRequest _request;

    internal IndexClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IdTokenResponse>> BaseAsync(
        IdTokenResponse defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("base").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IdTokenResponse>> IsAdminAsync(
        IdTokenResponse defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("isAdmin").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IdTokenResponse>> HasPaymentsAccessAsync(
        IdTokenResponse defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("hasPaymentsAccess").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IdTokenResponse>> HasCompanyPaymentsAccessAsync(
        Guid companyId,
        IdTokenResponse defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"hasCompanyPaymentsAccess/{companyId}").AsResponseAsync(defaultValue, ct);
    }
}
