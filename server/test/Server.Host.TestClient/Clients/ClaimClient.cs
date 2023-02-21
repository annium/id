using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;

namespace Server.Host.TestClient.Clients;

public class ClaimClient : ClientBase
{
    public ClaimClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IHttpResponse<IResult<Guid>>> CreateClaim(
        CreateClaimRequest body
    )
    {
        return await Request.Clone()
            .Post("claims")
            .JsonContent(body)
            .AsResponseAsync<IResult<Guid>>();
    }

    public async Task<IHttpResponse<IResult<ClaimResponse[]>>> ListClaims(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("claims")
            .Param("appId", appId)
            .AsResponseAsync<IResult<ClaimResponse[]>>();
    }

    public async Task<IHttpResponse<IResult>> UpdateClaim(
        Guid claimId,
        UpdateClaimRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"claims/{claimId}")
            .JsonContent(body)
            .AsResponseAsync<IResult>();
    }

    public async Task<IHttpResponse<IResult>> DeleteClaim(
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"claims/{claimId}")
            .AsResponseAsync<IResult>();
    }
}