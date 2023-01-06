using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Roles;
using Annium.Id.Api.ViewModels.Responses.Roles;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients;

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
            .AsAsync(Result.New(default(Guid)).Error("Request failed"));
    }

    public async Task<IResult<IEnumerable<RoleResponse>>> ListRoles(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("roles")
            .Param("appId", appId)
            .AsAsync(Result.New<IEnumerable<RoleResponse>>(Array.Empty<RoleResponse>()).Error("Request failed"));
    }

    public async Task<IResult> UpdateRole(
        Guid roleId,
        UpdateRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"roles/{roleId}")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> AddClaimToRole(
        Guid claimId,
        Guid roleId,
        AddClaimToRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> DeleteClaimFromRole(
        Guid claimId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"roles/{roleId}/claims/{claimId}")
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> DeleteRole(
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"roles/{roleId}")
            .AsAsync(Result.New().Error("Request failed"));
    }
}