using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Roles;
using Annium.Id.Api.ViewModels.Responses.Roles;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient;

public class RoleClient : ClientBase
{
    public RoleClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateRole(
        CreateRoleRequest body
    )
    {
        return await Request.Clone()
            .Post("roles")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<IEnumerable<RoleResponse>>>> ListRoles(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("roles")
            .Param("appId", appId)
            .AsResponseAsync<IResult<IEnumerable<RoleResponse>>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateRole(
        Guid roleId,
        UpdateRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"roles/{roleId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> AddClaimToRole(
        Guid claimId,
        Guid roleId,
        AddClaimToRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromRole(
        Guid claimId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"roles/{roleId}/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteRole(
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"roles/{roleId}")
            .AsResponseAsync<IResult>();
    }
}