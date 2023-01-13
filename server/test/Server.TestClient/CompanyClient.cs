using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using Server.ViewModels.Responses.Users;

namespace Server.TestClient;

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

    public async Task<IHttpResponse<IResult<IEnumerable<CompanyResponse>>>> FindCompanies(
        string query
    )
    {
        return await Request.Clone()
            .Get("companies")
            .Param("query", query)
            .AsResponseAsync<IResult<IEnumerable<CompanyResponse>>>();
    }

    public async Task<IHttpResponse<IResult<IEnumerable<CompanyResponse>>>> ListMyCompanies(
    )
    {
        return await Request.Clone()
            .Get("companies/my")
            .AsResponseAsync<IResult<IEnumerable<CompanyResponse>>>();
    }

    public async Task<IHttpResponse<IResult<CompanyResponse>>> GetCompany(
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