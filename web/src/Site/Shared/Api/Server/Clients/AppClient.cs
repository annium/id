using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;

namespace Site.Shared.Api.Server.Clients;

public class AppClient
{
    private readonly IHttpRequest _request;

    internal AppClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<Guid>> CreateApp(
        CreateAppRequest body
    )
    {
        return await _request
            .Post("apps")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<AppResponse[]>> FindApps(
        string query
    )
    {
        return await _request
            .Get("apps")
            .Param("query", query)
            .AsAsync<IResult<AppResponse[]>>();
    }

    public async Task<IResult<AppResponse[]>> ListMyApps(
    )
    {
        return await _request
            .Get("apps/my")
            .AsAsync<IResult<AppResponse[]>>();
    }

    public async Task<IResult<AppResponse>> GetApp(
        Guid appId
    )
    {
        return await _request
            .Get($"apps/{appId}")
            .AsAsync<IResult<AppResponse>>();
    }

    public async Task<IResult<Guid>> GetAppApiToken(
        Guid appId
    )
    {
        return await _request
            .Get($"apps/{appId}/token")
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult> UpdateApp(
        Guid appId,
        UpdateAppRequestBody body
    )
    {
        return await _request
            .Put($"apps/{appId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> SetAppOwner(
        Guid appId,
        Guid newOwnerId
    )
    {
        return await _request
            .Put($"apps/{appId}/owner/{newOwnerId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult<Guid>> UpdateAppApiToken(
        Guid appId
    )
    {
        return await _request
            .Put($"apps/{appId}/token")
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult> DeleteApp(
        Guid appId
    )
    {
        return await _request
            .Delete($"apps/{appId}")
            .AsAsync<IResult>();
    }
}