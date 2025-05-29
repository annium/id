using System;
using System.Threading;
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

    public async Task<IResult<Guid>> CreateAppAsync(
        CreateAppRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("apps").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<AppResponse[]>> FindAppsAsync(
        string query,
        IResult<AppResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("apps").Param("query", query).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<AppResponse[]>> ListMyAppsAsync(
        IResult<AppResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("apps/my").AsAsync(defaultValue, ct);
    }

    public async Task<IResult<AppResponse>> GetAppAsync(
        Guid appId,
        IResult<AppResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"apps/{appId}").AsAsync(defaultValue, ct);
    }

    public async Task<IResult<Guid>> GetAppApiTokenAsync(
        Guid appId,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"apps/{appId}/token").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateAppAsync(
        Guid appId,
        UpdateAppRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"apps/{appId}").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> SetAppOwnerAsync(
        Guid appId,
        Guid newOwnerId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"apps/{appId}/owner/{newOwnerId}").AsAsync(defaultValue, ct);
    }

    public async Task<IResult<Guid>> UpdateAppApiTokenAsync(
        Guid appId,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"apps/{appId}/token").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteAppAsync(Guid appId, IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete($"apps/{appId}").AsAsync(defaultValue, ct);
    }
}
