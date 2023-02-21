using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;

namespace Site.Shared.Api.Server.Clients;

public class RoleClient : ClientBase
{
    public RoleClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<Guid>> CreateRole(
        CreateRoleRequest body
    )
    {
        return await Request.Clone()
            .Post("roles")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<RoleResponse[]>> ListRoles(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("roles")
            .Param("appId", appId)
            .AsAsync<IResult<RoleResponse[]>>();
    }

    public async Task<IResult> UpdateRole(
        Guid roleId,
        UpdateRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"roles/{roleId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> AddClaimToRole(
        Guid roleId,
        Guid claimId,
        AddClaimToRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"roles/{roleId}/claims/{claimId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteRole(
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"roles/{roleId}")
            .AsAsync<IResult>();
    }
}