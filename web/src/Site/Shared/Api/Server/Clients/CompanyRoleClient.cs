using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyRoles;
using Server.ViewModels.Responses.CompanyRoles;

namespace Site.Shared.Api.Server.Clients;

public class CompanyRoleClient : ClientBase
{
    public CompanyRoleClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<Guid>> CreateRole(
        CreateCompanyRoleRequest body
    )
    {
        return await Request.Clone()
            .Post("companies/roles")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<CompanyRoleResponse[]>> ListRoles(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("companies/roles")
            .Param("appId", appId)
            .AsAsync<IResult<CompanyRoleResponse[]>>();
    }

    public async Task<IResult> UpdateRole(
        Guid roleId,
        UpdateCompanyRoleRequestBody body
    )
    {
        return await Request.Clone()
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
        return await Request.Clone()
            .Post($"companies/roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"companies/roles/{roleId}/claims/{claimId}")
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteRole(
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"companies/roles/{roleId}")
            .AsAsync<IResult>();
    }
}