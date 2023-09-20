using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using Server.ViewModels.Responses.Users;

namespace Server.Host.TestClient.Clients;

public class CompanyClient
{
    private readonly IHttpRequest _request;

    internal CompanyClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> RegisterCompany(
        RegisterCompanyRequest body
    )
    {
        return await _request
            .Post("companies")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<CompanyResponse[]>>> FindCompanies(
        string query
    )
    {
        return await _request
            .Get("companies")
            .Param("query", query)
            .AsResponseAsync<IResult<CompanyResponse[]>>();
    }

    public async Task<IHttpResponse<IResult<CompanyResponse[]>>> ListMyCompanies(
    )
    {
        return await _request
            .Get("companies/my")
            .AsResponseAsync<IResult<CompanyResponse[]>>();
    }

    public async Task<IHttpResponse<IResult<CompanyResponse>>> GetCompany(
        Guid companyId
    )
    {
        return await _request
            .Get($"companies/{companyId}")
            .AsResponseAsync<IResult<CompanyResponse>>();
    }

    public async Task<IHttpResponse<IResult<UserResponse[]>>> GetCompanyUsers(
        Guid companyId
    )
    {
        return await _request
            .Get($"companies/{companyId}/users")
            .AsResponseAsync<IResult<UserResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateCompany(
        Guid companyId,
        UpdateCompanyRequestBody body
    )
    {
        return await _request
            .Put($"companies/{companyId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> SetCompanyOwner(
        Guid companyId,
        Guid userId
    )
    {
        return await _request
            .Put($"companies/{companyId}/owner/{userId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> UnregisterCompany(
        Guid companyId
    )
    {
        return await _request
            .Delete($"companies/{companyId}")
            .AsResponseAsync<IResult>();
    }
}