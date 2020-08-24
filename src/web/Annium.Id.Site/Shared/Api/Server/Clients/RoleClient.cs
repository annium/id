using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Roles;
using Annium.Id.Api.ViewModels.Responses.Roles;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
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
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<Guid>>();
        }

        public async Task<IResult<IEnumerable<RoleResponse>>> ListRoles(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get("roles")
                .Param("appId", appId)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<RoleResponse>>>();
        }

        public async Task<IResult> UpdateRole(
            Guid roleId,
            UpdateRoleRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"roles/{roleId}")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
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
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteClaimFromRole(
            Guid claimId,
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"roles/{roleId}/claims/{claimId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteRole(
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"roles/{roleId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}