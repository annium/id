using System;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;
using static Server.TestClient.Helper;

namespace Server.TestClient;

public static class ClaimClientExtensions
{
    public static async Task<ClaimResponse> Register(
        this ClaimClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateClaimRequest { AppId = appId, Key = key ?? Faker.Random.String(5), Name = name ?? Faker.Random.String(10) };
        var roleId = await client.CreateClaim(request).GetData();
        var roles = await client.ListClaims(appId).GetData();

        return roles.Single(x => x.Id == roleId);
    }
}