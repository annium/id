using System;
using System.Threading;
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

    public async Task<IHttpResponse<IResult<Guid>>> CreateRoleAsync(
        CreateCompanyRoleRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("companies/roles").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<CompanyRoleResponse[]>>> ListRolesAsync(
        Guid appId,
        IResult<CompanyRoleResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies/roles").Param("appId", appId).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateRoleAsync(
        Guid roleId,
        UpdateCompanyRoleRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/roles/{roleId}").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> AddClaimToRoleAsync(
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
            .AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteClaimFromRoleAsync(
        Guid roleId,
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"companies/roles/{roleId}/claims/{claimId}").AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteRoleAsync(
        Guid roleId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"companies/roles/{roleId}").AsResponseAsync(defaultValue, ct);
    }
}
