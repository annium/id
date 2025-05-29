using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class ClaimClientExtensions
{
    public static async Task<ClaimResponse> RegisterAsync(
        this ClaimClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateClaimRequest
        {
            AppId = appId,
            Key = key ?? Faker.Random.String2(5),
            Name = name ?? Faker.Random.String2(10),
        };
        var claimId = await client
            .CreateClaimAsync(request, Result.New(Guid.Empty).Error("Failed to create claim"))
            .GetDataAsync();
        var claims = await client
            .ListClaimsAsync(appId, Result.New(Array.Empty<ClaimResponse>()).Error("Failed to list claims"))
            .GetDataAsync();

        return claims.Single(x => x.Id == claimId);
    }
}
