using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;

namespace Site.Shared.Api.Server.Clients;

public class ClaimClient
{
    private readonly IHttpRequest _request;

    internal ClaimClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IResult<Guid>> CreateClaim(
        CreateClaimRequest body,
        IResult<Guid> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Post("claims")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult<ClaimResponse[]>> ListClaims(
        Guid appId,
        IResult<ClaimResponse[]> defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Get("claims")
            .Param("appId", appId)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> UpdateClaim(
        Guid claimId,
        UpdateClaimRequestBody body,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Put($"claims/{claimId}")
            .JsonContent(body)
            .AsAsync(defaultValue, ct);
    }

    public async Task<IResult> DeleteClaim(
        Guid claimId,
        IResult defaultValue,
        CancellationToken ct = default
    )
    {
        return await _request
            .Delete($"claims/{claimId}")
            .AsAsync(defaultValue, ct);
    }
}