using System;
using System.Threading;
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
        CreateAppRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post("apps")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<AppResponse[]>>> FindApps(
        string query,
        IResult<AppResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("apps")
            .Param("query", query)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<AppResponse[]>>> ListMyApps(
        IResult<AppResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("apps/my")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<AppResponse>>> GetApp(
        Guid appId,
        IResult<AppResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get($"apps/{appId}")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<Guid>>> GetAppApiToken(
        Guid appId,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get($"apps/{appId}/token")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateApp(
        Guid appId,
        UpdateAppRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put($"apps/{appId}")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> SetAppOwner(
        Guid appId,
        Guid newOwnerId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put($"apps/{appId}/owner/{newOwnerId}")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<Guid>>> UpdateAppApiToken(
        Guid appId,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put($"apps/{appId}/token")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteApp(
        Guid appId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"apps/{appId}")
            .AsResponseAsync(defaultValue, ct);
    }
}