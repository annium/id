using System;
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

    public async Task<IHttpResponse<IResult>> AddUserToCompany(
        Guid companyId,
        Guid userId
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> AddCompanyRoleToCompanyUser(
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyRoleFromCompanyUser(
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/roles/{roleId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> AddCompanyClaimToCompanyUser(
        Guid companyId,
        Guid userId,
        Guid claimId,
        AddCompanyClaimToCompanyUserRequestBody body
    )
    {
        return await _request
            .Post($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyClaimFromCompanyUser(
        Guid companyId,
        Guid userId,
        Guid claimId
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteUserFromCompany(
        Guid companyId,
        Guid userId
    )
    {
        return await _request
            .Delete($"companies/{companyId}/users/{userId}")
            .AsResponseAsync<IResult>();
    }
}