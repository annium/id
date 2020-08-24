using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.CompanyUsers;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
    public class CompanyUserClient : ClientBase
    {
        public CompanyUserClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IResult> AddUserToCompany(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Post($"companies/{companyId}/users/{userId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> AddCompanyRoleToCompanyUser(
            Guid companyId,
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Post($"companies/{companyId}/users/{userId}/roles/{roleId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteCompanyRoleFromCompanyUser(
            Guid companyId,
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId}/users/{userId}/roles/{roleId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> AddCompanyClaimToCompanyUser(
            Guid claimId,
            Guid companyId,
            Guid userId,
            AddCompanyClaimToCompanyUserRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"companies/{companyId}/users/{userId}/claims/{claimId}")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteCompanyClaimFromCompanyUser(
            Guid claimId,
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId}/users/{userId}/claims/{claimId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> DeleteUserFromCompany(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId}/users/{userId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}