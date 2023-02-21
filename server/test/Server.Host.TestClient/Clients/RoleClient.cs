using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;

namespace Server.Host.TestClient.Clients;

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

    public async Task<IHttpResponse<IResult<RoleResponse[]>>> ListRoles(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("roles")
            .Param("appId", appId)
            .AsResponseAsync<IResult<RoleResponse[]>>();
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
        Guid roleId,
        Guid claimId,
        AddClaimToRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId
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