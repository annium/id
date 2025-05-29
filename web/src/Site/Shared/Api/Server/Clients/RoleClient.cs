using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;

namespace Site.Shared.Api.Server.Clients;

public class RoleClient
{
    private readonly IHttpRequest _request;

    internal RoleClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<Guid>> CreateRoleAsync(
        CreateRoleRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("roles").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<RoleResponse[]>> ListRolesAsync(
        Guid appId,
        IResult<RoleResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("roles").Param("appId", appId).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateRoleAsync(
        Guid roleId,
        UpdateRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"roles/{roleId}").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> AddClaimToRoleAsync(
        Guid roleId,
        Guid claimId,
        AddClaimToRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"roles/{roleId}/claims/{claimId}").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteClaimFromRoleAsync(
        Guid roleId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"roles/{roleId}/claims/{claimId}").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteRoleAsync(Guid roleId, IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete($"roles/{roleId}").AsAsync(defaultValue, ct);
    }
}
