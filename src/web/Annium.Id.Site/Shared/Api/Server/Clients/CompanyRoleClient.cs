using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.CompanyRoles;
using Annium.Id.Api.ViewModels.Responses.CompanyRoles;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
    public class CompanyRoleClient : ClientBase
    {
        public CompanyRoleClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IResult<Guid>> CreateRole(
            CreateCompanyRoleRequest body
        )
        {
            return await Request.Clone()
                .Post("companies/roles")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<Guid>>();
        }

        public async Task<IResult<IEnumerable<CompanyRoleResponse>>> ListRoles(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get("companies/roles")
                .Param("appId", appId)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<CompanyRoleResponse>>>();
        }

        public async Task<IResult> UpdateRole(
            Guid roleId,
            UpdateCompanyRoleRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"companies/roles/{roleId}")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> AddClaimToRole(
            Guid claimId,
            Guid roleId,
            AddCompanyClaimToCompanyRoleRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"companies/roles/{roleId}/claims/{claimId}")
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
                .Delete($"companies/roles/{roleId}/claims/{claimId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteRole(
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"companies/roles/{roleId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}