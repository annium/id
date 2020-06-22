using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.CompanyUsers.Requests;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
    public class CompanyUserClient : ClientBase
    {
        public CompanyUserClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IResult>> AddUserToCompany(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Post($"companies/{companyId:guid}/users/{userId:guid}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> AddCompanyRoleToCompanyUser(
            Guid companyId,
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Post($"companies/{companyId:guid}/users/{userId:guid}/roles/{roleId:guid}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteCompanyRoleFromCompanyUser(
            Guid companyId,
            Guid roleId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId:guid}/users/{userId:guid}/roles/{roleId:guid}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> AddCompanyClaimToCompanyUser(
            Guid claimId,
            Guid companyId,
            Guid userId,
            AddCompanyClaimToCompanyUserRequestBody body
        )
        {
            return await Request.Clone()
                .Post($"companies/{companyId:guid}/users/{userId:guid}/claims/{claimId:guid}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteCompanyClaimFromCompanyUser(
            Guid claimId,
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId:guid}/users/{userId:guid}/claims/{claimId:guid}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> DeleteUserFromCompany(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId:guid}/users/{userId:guid}")
                .AsResponseAsync<IResult>();
        }
    }
}