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
        var request = new CreateCompanyClaimRequest { AppId = appId, Key = key ?? Faker.Random.String(5), Name = name ?? Faker.Random.String(10) };
        var roleId = await client.CreateCompanyClaim(request).GetData();
        var roles = await client.ListCompanyClaims(appId).GetData();

        return roles.Single(x => x.Id == roleId);
    }
}