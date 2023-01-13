using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;

namespace Site.Shared.Api.Server.Clients;

public class ClaimClient : ClientBase
{
    public ClaimClient(IHttpRequest request) : base(request)
    {
    }

    public async Task<IResult<Guid>> CreateClaim(
        CreateClaimRequest body
    )
    {
        return await Request.Clone()
            .Post("claims")
            .JsonContent(body)
            .AsAsync(Result.New(default(Guid)).Error("Request failed"));
    }

    public async Task<IResult<IEnumerable<ClaimResponse>>> ListClaims(
        Guid appId
    )
    {
        return await Request.Clone()
            .Get("claims")
            .Param("appId", appId)
            .AsAsync(Result.New<IEnumerable<ClaimResponse>>(Array.Empty<ClaimResponse>()).Error("Request failed"));
    }

    public async Task<IResult> UpdateClaim(
        Guid claimId,
        UpdateClaimRequestBody body
    )
    {
        return await Request.Clone()
            .Put($"claims/{claimId}")
            .JsonContent(body)
            .AsAsync(Result.New().Error("Request failed"));
    }

    public async Task<IResult> DeleteClaim(
        Guid claimId
    )
    {
        return await Request.Clone()
            .Delete($"claims/{claimId}")
            .AsAsync(Result.New().Error("Request failed"));
    }
}