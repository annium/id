using System;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;

namespace Server.TestClient;

public static class ClaimClientExtensions
{
    public static async Task<ClaimResponse> Register(
        this ClaimClient client,
        Guid appId,
        string key = "one",
        string name = "First claim"
    )
    {
        var request = new CreateClaimRequest { AppId = appId, Key = key, Name = name };
        var roleId = await client.CreateClaim(request).GetData();
        var roles = await client.ListClaims(appId).GetData();

        return roles.Single(x => x.Id == roleId);
    }

    public static Task<ClaimResponse> RegisterOther(
        this ClaimClient client,
        Guid appId,
        string key = "two",
        string name = "Second claim"
    )
    {
        return client.Register(appId, key, name);
    }
}