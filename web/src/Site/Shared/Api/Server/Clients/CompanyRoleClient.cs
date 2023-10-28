using System;
using System.Threading;
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
        CreateCompanyRoleRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("companies/roles").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult<CompanyRoleResponse[]>> ListRoles(
        Guid appId,
        IResult<CompanyRoleResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies/roles").Param("appId", appId).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateRole(
        Guid roleId,
        UpdateCompanyRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/roles/{roleId}").JsonContent(body).AsAsync(defaultValue, ct);
    }

    public async Task<IResult> AddClaimToRole(
        Guid roleId,
        Guid claimId,
        AddCompanyClaimToCompanyRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post($"companies/roles/{roleId}/claims/{claimId}")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteClaimFromRole(
        Guid roleId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"companies/roles/{roleId}/claims/{claimId}").AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteRole(Guid roleId, IResult defaultValue, CancellationToken ct = default)
    {
        return await _request.Delete($"companies/roles/{roleId}").AsAsync(defaultValue, ct);
    }
}
