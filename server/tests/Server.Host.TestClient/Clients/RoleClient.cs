using System;
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

    public async Task<IHttpResponse<IResult<Guid>>> CreateRole(
        CreateRoleRequest body
    )
    {
        return await _request
            .Post("roles")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<RoleResponse[]>>> ListRoles(
        Guid appId
    )
    {
        return await _request
            .Get("roles")
            .Param("appId", appId)
            .AsResponseAsync<IResult<RoleResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateRole(
        Guid roleId,
        UpdateRoleRequestBody body
    )
    {
        return await _request
            .Put($"roles/{roleId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> AddClaimToRole(
        Guid roleId,
        Guid claimId,
        AddClaimToRoleRequestBody body
    )
    {
        return await _request
            .Post($"roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId
    )
    {
        return await _request
            .Delete($"roles/{roleId}/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteRole(
        Guid roleId
    )
    {
        return await _request
            .Delete($"roles/{roleId}")
            .AsResponseAsync<IResult>();
    }
}