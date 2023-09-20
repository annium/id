using System;
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
        Guid userId
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> AddCompanyRoleToCompanyUser(
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteCompanyRoleFromCompanyUser(
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> AddCompanyClaimToCompanyUser(
        Guid companyId,
        Guid userId,
        Guid claimId,
        AddCompanyClaimToCompanyUserRequestBody body
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteCompanyClaimFromCompanyUser(
        Guid companyId,
        Guid userId,
        Guid claimId
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteUserFromCompany(
        Guid companyId,
        Guid userId
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}")
            .AsAsync<IResult>();
    }
}