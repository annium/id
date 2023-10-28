using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.Host.TestClient.Clients;

public class CompanyClaimClient
{
    private readonly IHttpRequest _request;

    internal CompanyClaimClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateCompanyClaim(
        CreateCompanyClaimRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Post("companies/claims").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult<CompanyClaimResponse[]>>> ListCompanyClaims(
        Guid appId,
        IResult<CompanyClaimResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Get("companies/claims").Param("appId", appId).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> UpdateCompanyClaim(
        Guid claimId,
        UpdateCompanyClaimRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Put($"companies/claims/{claimId}").JsonContent(body).AsResponseAsync(defaultValue, ct);
    }

    public async Task<IHttpResponse<IResult>> DeleteCompanyClaim(
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request.Delete($"companies/claims/{claimId}").AsResponseAsync(defaultValue, ct);
    }
}
