using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using Server.ViewModels.Responses.Users;

namespace Site.Shared.Api.Server.Clients;

public class CompanyClient
{
    private readonly IHttpRequest _request;

    internal CompanyClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<Guid>> RegisterCompany(
        RegisterCompanyRequest body
    )
    {
        return await _request
            .Post("companies")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<CompanyResponse[]>> FindCompanies(
        string query
    )
    {
        return await _request
            .Get("companies")
            .Param("query", query)
            .AsAsync<IResult<CompanyResponse[]>>();
    }

    public async Task<IResult<CompanyResponse[]>> ListMyCompanies(
    )
    {
        return await _request
            .Get("companies/my")
            .AsAsync<IResult<CompanyResponse[]>>();
    }

    public async Task<IResult<CompanyResponse>> GetCompany(
        Guid companyId
    )
    {
        return await _request
            .Get($"companies/{companyId}")
            .AsAsync<IResult<CompanyResponse>>();
    }

    public async Task<IResult<UserResponse[]>> GetCompanyUsers(
        Guid companyId
    )
    {
        return await _request
            .Get($"companies/{companyId}/users")
            .AsAsync<IResult<UserResponse[]>>();
    }

    public async Task<IResult> UpdateCompany(
        Guid companyId,
        UpdateCompanyRequestBody body
    )
    {
        return await _request
            .Put($"companies/{companyId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> SetCompanyOwner(
        Guid companyId,
        Guid userId
    )
    {
        return await _request
            .Put($"companies/{companyId}/owner/{userId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> UnregisterCompany(
        Guid companyId
    )
    {
        return await _request
            .Delete($"companies/{companyId}")
            .AsAsync<IResult>();
    }
}