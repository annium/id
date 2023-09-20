using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyRoles;
using Server.ViewModels.Responses.CompanyRoles;

namespace Site.Shared.Api.Server.Clients;

public class CompanyRoleClient
{
    private readonly IHttpRequest _request;

    internal CompanyRoleClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<Guid>> CreateRole(
        CreateCompanyRoleRequest body
    )
    {
        return await _request
            .Post("companies/roles")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<CompanyRoleResponse[]>> ListRoles(
        Guid appId
    )
    {
        return await _request
            .Get("companies/roles")
            .Param("appId", appId)
            .AsAsync<IResult<CompanyRoleResponse[]>>();
    }

    public async Task<IResult> UpdateRole(
        Guid roleId,
        UpdateCompanyRoleRequestBody body
    )
    {
        return await _request
            .Put($"companies/roles/{roleId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> AddClaimToRole(
        Guid roleId,
        Guid claimId,
        AddCompanyClaimToCompanyRoleRequestBody body
    )
    {
        return await _request
            .Post($"companies/roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId
    )
    {
        return await _request
            .Delete($"companies/roles/{roleId}/claims/{claimId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteRole(
        Guid roleId
    )
    {
        return await _request
            .Delete($"companies/roles/{roleId}")
            .AsAsync<IResult>();
    }
}