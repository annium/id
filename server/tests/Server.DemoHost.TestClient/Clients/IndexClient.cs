using System;
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

    public async Task<IHttpResponse<IdTokenResponse>> Base(
    )
    {
        return await _request
            .Get("base")
            .AsResponseAsync<IdTokenResponse>();
    }

    public async Task<IHttpResponse<IdTokenResponse>> IsAdmin(
    )
    {
        return await _request
            .Get("isAdmin")
            .AsResponseAsync<IdTokenResponse>();
    }

    public async Task<IHttpResponse<IdTokenResponse>> HasPaymentsAccess(
    )
    {
        return await _request
            .Get("hasPaymentsAccess")
            .AsResponseAsync<IdTokenResponse>();
    }

    public async Task<IHttpResponse<IdTokenResponse>> HasCompanyPaymentsAccess(
        Guid companyId
    )
    {
        return await _request
            .Get($"hasCompanyPaymentsAccess/{companyId}")
            .AsResponseAsync<IdTokenResponse>();
    }
}