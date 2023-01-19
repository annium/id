using System;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;
using static Server.TestClient.Helper;

namespace Server.TestClient;

public static class CompanyClaimClientExtensions
{
    public static async Task<CompanyClaimResponse> Register(
        this CompanyClaimClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateCompanyClaimRequest { AppId = appId, Key = key ?? Faker.Random.String2(5), Name = name ?? Faker.Random.String2(10) };
        var claimId = await client.CreateCompanyClaim(request).GetData();
        var claims = await client.ListCompanyClaims(appId).GetData();

        return claims.Single(x => x.Id == claimId);
    }
}