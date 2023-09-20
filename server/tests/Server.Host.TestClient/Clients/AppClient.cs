using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;

namespace Server.Host.TestClient.Clients;

public class AppClient
{
    private readonly IHttpRequest _request;

    internal AppClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateApp(
        CreateAppRequest body
    )
    {
        return await _request
            .Post("apps")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<AppResponse[]>>> FindApps(
        string query
    )
    {
        return await _request
            .Get("apps")
            .Param("query", query)
            .AsResponseAsync<IResult<AppResponse[]>>();
    }

    public async Task<IHttpResponse<IResult<AppResponse[]>>> ListMyApps(
    )
    {
        return await _request
            .Get("apps/my")
            .AsResponseAsync<IResult<AppResponse[]>>();
    }

    public async Task<IHttpResponse<IResult<AppResponse>>> GetApp(
        Guid appId
    )
    {
        return await _request
            .Get($"apps/{appId}")
            .AsResponseAsync<IResult<AppResponse>>();
    }

    public async Task<IHttpResponse<IResult<Guid>>> GetAppApiToken(
        Guid appId
    )
    {
        return await _request
            .Get($"apps/{appId}/token")
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateApp(
        Guid appId,
        UpdateAppRequestBody body
    )
    {
        return await _request
            .Put($"apps/{appId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> SetAppOwner(
        Guid appId,
        Guid newOwnerId
    )
    {
        return await _request
            .Put($"apps/{appId}/owner/{newOwnerId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult<Guid>>> UpdateAppApiToken(
        Guid appId
    )
    {
        return await _request
            .Put($"apps/{appId}/token")
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult>> DeleteApp(
        Guid appId
    )
    {
        return await _request
            .Delete($"apps/{appId}")
            .AsResponseAsync<IResult>();
    }
}