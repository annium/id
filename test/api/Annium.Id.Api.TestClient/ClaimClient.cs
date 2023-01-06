using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Claims;
using Annium.Id.Api.ViewModels.Responses.Claims;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient;

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

    public async Task<IHttpResponse<IResult<IEnumerable<ClaimResponse>>>> ListClaims(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("claims")
            .Param("appId", appId)
            .AsResponseAsync<IResult<IEnumerable<ClaimResponse>>>();
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