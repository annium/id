using System;
using System.Threading;
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
        RegisterCompanyRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("companies").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<CompanyResponse[]>> FindCompanies(
        string query,
        IResult<CompanyResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies").Param("query", query).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<CompanyResponse[]>> ListMyCompanies(
        IResult<CompanyResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies/my").AsAsync(defaultValue, ct);
    }

    public async Task<IResult<CompanyResponse>> GetCompany(
        Guid companyId,
        IResult<CompanyResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"companies/{companyId}").AsAsync(defaultValue, ct);
    }

    public async Task<IResult<UserResponse[]>> GetCompanyUsers(
        Guid companyId,
        IResult<UserResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"companies/{companyId}/users").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateCompany(
        Guid companyId,
        UpdateCompanyRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/{companyId}").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> SetCompanyOwner(
        Guid companyId,
        Guid userId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/{companyId}/owner/{userId}").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UnregisterCompany(Guid companyId, IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete($"companies/{companyId}").AsAsync(defaultValue, ct);
    }
}
