using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;

namespace Server.Host.TestClient.Clients;

public class ClaimClient
{
    private readonly IHttpRequest _request;

    internal ClaimClient(IHttpRequest request)
    {
        _request = request;
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateClaim(
        CreateClaimRequest body
    )
    {
        return await _request
            .Post("claims")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<ClaimResponse[]>>> ListClaims(
        Guid appId
    )
    {
        return await _request
            .Get("claims")
            .Param("appId", appId)
            .AsResponseAsync<IResult<ClaimResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateClaim(
        Guid claimId,
        UpdateClaimRequestBody body
    )
    {
        return await _request
            .Put($"claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaim(
        Guid claimId
    )
    {
        return await _request
            .Delete($"claims/{claimId}")
            .AsResponseAsync<IResult>();
    }
}