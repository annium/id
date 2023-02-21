using System;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;
using static Server.Host.TestClient.Helper;

namespace Server.Host.TestClient;

public static class ClaimClientExtensions
{
    public static async Task<ClaimResponse> Register(
        this ClaimClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateClaimRequest { AppId = appId, Key = key ?? Faker.Random.String2(5), Name = name ?? Faker.Random.String2(10) };
        var claimId = await client.CreateClaim(request).GetData();
        var claims = await client.ListClaims(appId).GetData();

        return claims.Single(x => x.Id == claimId);
    }
}