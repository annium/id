using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Roles.Requests;
using Annium.Id.Api.ViewModels.Roles.Responses;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
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
                .Put($"roles/{roleId:guid}")
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
                .Post($"roles/{roleId:guid}/claims/{claimId:guid}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteClaimFromRole(
            Guid claimId,
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"roles/{roleId:guid}/claims/{claimId:guid}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteRole(
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"roles/{roleId:guid}")
                .AsResponseAsync<IResult>();
        }
    }
}