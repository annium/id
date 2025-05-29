using System;
using System.Threading;
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

    public async Task<IHttpResponse<IResult<Guid>>> RegisterCompanyAsync(
        RegisterCompanyRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("companies").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<CompanyResponse[]>>> FindCompaniesAsync(
        string query,
        IResult<CompanyResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies").Param("query", query).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<CompanyResponse[]>>> ListMyCompaniesAsync(
        IResult<CompanyResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies/my").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<CompanyResponse>>> GetCompanyAsync(
        Guid companyId,
        IResult<CompanyResponse> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"companies/{companyId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<UserResponse[]>>> GetCompanyUsersAsync(
        Guid companyId,
        IResult<UserResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get($"companies/{companyId}/users").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateCompanyAsync(
        Guid companyId,
        UpdateCompanyRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/{companyId}").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> SetCompanyOwnerAsync(
        Guid companyId,
        Guid userId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/{companyId}/owner/{userId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UnregisterCompanyAsync(
        Guid companyId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"companies/{companyId}").AsResponseAsync(defaultValue, ct);
    }
}
