using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;

namespace Server.Host.TestClient.Clients;

public class RoleClient
{
    private readonly IHttpRequest _request;

    internal RoleClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateRoleAsync(
        CreateRoleRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("roles").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<RoleResponse[]>>> ListRolesAsync(
        Guid appId,
        IResult<RoleResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("roles").Param("appId", appId).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateRoleAsync(
        Guid roleId,
        UpdateRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"roles/{roleId}").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> AddClaimToRoleAsync(
        Guid roleId,
        Guid claimId,
        AddClaimToRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromRoleAsync(
        Guid roleId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"roles/{roleId}/claims/{claimId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteRoleAsync(
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"roles/{roleId}").AsResponseAsync(defaultValue, ct);
    }
}
