using System;
using System.Collections.Generic;
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
            .AsAsync(Result.New(default(Guid)).Error("Request failed"));
    }

    public async Task<IResult<IEnumerable<CompanyRoleResponse>>> ListRoles(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("companies/roles")
            .Param("appId", appId)
            .AsAsync(Result.New<IEnumerable<CompanyRoleResponse>>(Array.Empty<CompanyRoleResponse>()).Error("Request failed"));
    }

    public async Task<IResult> UpdateRole(
        Guid roleId,
        UpdateCompanyRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"companies/roles/{roleId}")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> AddClaimToRole(
        Guid claimId,
        Guid roleId,
        AddCompanyClaimToCompanyRoleRequestBody body
    )
    {
        return await Request.Clone()
            .Post($"companies/roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> DeleteClaimFromRole(
        Guid claimId,
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"companies/roles/{roleId}/claims/{claimId}")
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> DeleteRole(
        Guid roleId
    )
    {
        return await Request.Clone()
            .Delete($"companies/roles/{roleId}")
            .AsAsync(Result.New().Error("Request failed"));
    }
}