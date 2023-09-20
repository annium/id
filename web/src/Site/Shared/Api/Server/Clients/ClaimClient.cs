using System;
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
        CreateClaimRequest body
    )
    {
        return await _request
            .Post("claims")
            .JsonContent(body)
            .AsAsync<IResult<Guid>>();
    }

    public async Task<IResult<ClaimResponse[]>> ListClaims(
        Guid appId
    )
    {
        return await _request
            .Get("claims")
            .Param("appId", appId)
            .AsAsync<IResult<ClaimResponse[]>>();
    }

    public async Task<IResult> UpdateClaim(
        Guid claimId,
        UpdateClaimRequestBody body
    )
    {
        return await _request
            .Put($"claims/{claimId}")
            .JsonContent(body)
            .AsAsync<IResult>();
    }

    public async Task<IResult> DeleteClaim(
        Guid claimId
    )
    {
        return await _request
            .Delete($"claims/{claimId}")
            .AsAsync<IResult>();
    }
}