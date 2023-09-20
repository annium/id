using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyRoles;
using Server.ViewModels.Responses.CompanyRoles;

namespace Server.Host.TestClient.Clients;

public class CompanyRoleClient
{
    private readonly IHttpRequest _request;

    internal CompanyRoleClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateRole(
        CreateCompanyRoleRequest body
    )
    {
        return await _request
            .Post("companies/roles")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<CompanyRoleResponse[]>>> ListRoles(
        Guid appId
    )
    {
        return await _request
            .Get("companies/roles")
            .Param("appId", appId)
            .AsResponseAsync<IResult<CompanyRoleResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateRole(
        Guid roleId,
        UpdateCompanyRoleRequestBody body
    )
    {
        return await _request
            .Put($"companies/roles/{roleId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> AddClaimToRole(
        Guid roleId,
        Guid claimId,
        AddCompanyClaimToCompanyRoleRequestBody body
    )
    {
        return await _request
            .Post($"companies/roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId
    )
    {
        return await _request
            .Delete($"companies/roles/{roleId}/claims/{claimId}")
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteRole(
        Guid roleId
    )
    {
        return await _request
            .Delete($"companies/roles/{roleId}")
            .AsResponseAsync<IResult>();
    }
}