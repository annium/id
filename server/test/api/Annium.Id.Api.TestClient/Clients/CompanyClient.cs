using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Companies.Requests;
using Annium.Id.Api.ViewModels.Companies.Responses;
using Annium.Id.Api.ViewModels.Users.Responses;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient.Clients
{
    public class CompanyClient : ClientBase
    {
        public CompanyClient(IHttpRequest request) : base(request)
        {
        }

        public async Task<IHttpResponse<IResult<Guid>>> RegisterCompany(
            RegisterCompanyRequest body
        )
        {
            return await Request.Clone()
                .Post("companies")
                .JsonContent(body)
                .AsResponseAsync<IResult<Guid>>();
        }

        public async Task<IHttpResponse<IResult<CompanyResponse>>> GetCompanyInfo(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"companies/{companyId}")
                .AsResponseAsync<IResult<CompanyResponse>>();
        }

        public async Task<IHttpResponse<IResult<IEnumerable<UserResponse>>>> GetCompanyUsers(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Get($"companies/{companyId}/users")
                .AsResponseAsync<IResult<IEnumerable<UserResponse>>>();
        }

        public async Task<IHttpResponse<IResult>> UpdateCompany(
            Guid companyId,
            UpdateCompanyRequestBody body
        )
        {
            return await Request.Clone()
                .Put($"companies/{companyId}")
                .JsonContent(body)
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> SetCompanyOwner(
            Guid companyId,
            Guid userId
        )
        {
            return await Request.Clone()
                .Put($"companies/{companyId}/owner/{userId}")
                .AsResponseAsync<IResult>();
        }

        public async Task<IHttpResponse<IResult>> UnregisterCompany(
            Guid companyId
        )
        {
            return await Request.Clone()
                .Delete($"companies/{companyId}")
                .AsResponseAsync<IResult>();
        }
    }
}