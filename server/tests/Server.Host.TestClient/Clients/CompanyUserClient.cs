using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyUsers;

namespace Server.Host.TestClient.Clients;

public class CompanyUserClient
{
    private readonly IHttpRequest _request;

    internal CompanyUserClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult>> AddUserToCompanyAsync(
        Guid companyId,
        Guid userId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post($"companies/{companyId}/users/{userId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> AddCompanyRoleToCompanyUserAsync(
        Guid companyId,
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyRoleFromCompanyUserAsync(
        Guid companyId,
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> AddCompanyClaimToCompanyUserAsync(
        Guid companyId,
        Guid userId,
        Guid claimId,
        AddCompanyClaimToCompanyUserRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyClaimFromCompanyUserAsync(
        Guid companyId,
        Guid userId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteUserFromCompanyAsync(
        Guid companyId,
        Guid userId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"companies/{companyId}/users/{userId}").AsResponseAsync(defaultValue, ct);
    }
}
