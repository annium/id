using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Companies;
using Annium.Id.Api.ViewModels.Responses.Companies;
using Annium.Id.Api.ViewModels.Responses.Users;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server.Clients
{
    public class CompanyClient : ClientBase
    {
        public CompanyClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IResult<Guid>> RegisterCompany(
            RegisterCompanyRequest body
        )
        {
            return await Request.Clone()
                .Post("companies")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<Guid>>();
        }

        public async Task<IResult<IEnumerable<CompanyResponse>>> FindCompanies(
            string query
        )
        {
            return await Request.Clone()
                .Get("companies")
                .Param("query", query)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<CompanyResponse>>>();
        }

        public async Task<IResult<IEnumerable<CompanyResponse>>> ListMyCompanies(
        )
        {
            return await Request.Clone()
                .Get("companies/my")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<CompanyResponse>>>();
        }

        public async Task<IResult<CompanyResponse>> GetCompany(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"companies/{companyId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<CompanyResponse>>();
        }

        public async Task<IResult<IEnumerable<UserResponse>>> GetCompanyUsers(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"companies/{companyId}/users")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult<IEnumerable<UserResponse>>>();
        }

        public async Task<IResult> UpdateCompany(
            Guid companyId,
            UpdateCompanyRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"companies/{companyId}")
                .JsonContent(body)
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> SetCompanyOwner(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Put($"companies/{companyId}/owner/{userId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }

        public async Task<IResult> UnregisterCompany(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId}")
                .EnsureSuccessStatusCode()
                .AsAsync<IResult>();
        }
    }
}