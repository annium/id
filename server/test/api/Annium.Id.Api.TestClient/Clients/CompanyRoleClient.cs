using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.CompanyRoles;
using Annium.Id.Api.ViewModels.Responses.CompanyRoles;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
    public class CompanyRoleClient : ClientBase
    {
        public CompanyRoleClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IResult<Guid>>> CreateRole(
            CreateCompanyRoleRequest body
        )
        {
            return await Request.Clone()
                .Post("companies/roles")
                .JsonContent(body)
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult<IEnumerable<CompanyRoleResponse>>>> ListRoles(
            Guid appId
        )
        {
            return await Request.Clone()
                .Get("companies/roles")
                .Param("appId", appId)
                .AsResponseAsync<IResult<IEnumerable<CompanyRoleResponse>>>();
        }

        public async Task<IHttpResponse<IResult>> UpdateRole(
            Guid roleId,
            UpdateCompanyRoleRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"companies/roles/{roleId}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> AddClaimToRole(
            Guid claimId,
            Guid roleId,
            AddCompanyClaimToCompanyRoleRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"companies/roles/{roleId}/claims/{claimId}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteClaimFromRole(
            Guid claimId,
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"companies/roles/{roleId}/claims/{claimId}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteRole(
            Guid roleId
        )
        {
            return await Request.Clone()
                .Delete($"companies/roles/{roleId}")
                .AsResponseAsync<IResult>();
        }
    }
}