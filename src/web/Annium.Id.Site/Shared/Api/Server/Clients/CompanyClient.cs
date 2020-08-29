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
                .AsAsync(Result.New(default(Guid)).Error("Request failed"));
        }

        public async Task<IResult<IEnumerable<CompanyResponse>>> FindCompanies(
            string query
        )
        {
            return await Request.Clone()
                .Get("companies")
                .Param("query", query)
                .AsAsync(Result.New<IEnumerable<CompanyResponse>>(Array.Empty<CompanyResponse>()).Error("Request failed"));
        }

        public async Task<IResult<IEnumerable<CompanyResponse>>> ListMyCompanies(
        )
        {
            return await Request.Clone()
                .Get("companies/my")
                .AsAsync(Result.New<IEnumerable<CompanyResponse>>(Array.Empty<CompanyResponse>()).Error("Request failed"));
        }

        public async Task<IResult<CompanyResponse>> GetCompany(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"companies/{companyId}")
                .AsAsync(Result.New(new CompanyResponse()).Error("Request failed"));
        }

        public async Task<IResult<IEnumerable<UserResponse>>> GetCompanyUsers(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"companies/{companyId}/users")
                .AsAsync(Result.New<IEnumerable<UserResponse>>(Array.Empty<UserResponse>()).Error("Request failed"));
        }

        public async Task<IResult> UpdateCompany(
            Guid companyId,
            UpdateCompanyRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"companies/{companyId}")
                .JsonContent(body)
                .AsAsync(Result.New().Error("Request failed"));
        }

        public async Task<IResult> SetCompanyOwner(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Put($"companies/{companyId}/owner/{userId}")
                .AsAsync(Result.New().Error("Request failed"));
        }

        public async Task<IResult> UnregisterCompany(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId}")
                .AsAsync(Result.New().Error("Request failed"));
        }
    }
}