using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyUsers;

namespace Site.Shared.Api.Server.Clients;

public class CompanyUserClient
{
    private readonly IHttpRequest _request;

    internal CompanyUserClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult> AddUserToCompany(
        Guid companyId,
        Guid userId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> AddCompanyRoleToCompanyUser(
        Guid companyId,
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteCompanyRoleFromCompanyUser(
        Guid companyId,
        Guid userId,
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> AddCompanyClaimToCompanyUser(
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
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteCompanyClaimFromCompanyUser(
        Guid companyId,
        Guid userId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteUserFromCompany(
        Guid companyId,
        Guid userId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}")
            .AsAsync(defaultValue, ct);
    }
}