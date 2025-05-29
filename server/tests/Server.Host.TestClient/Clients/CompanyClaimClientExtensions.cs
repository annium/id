using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class CompanyClaimClientExtensions
{
    public static async Task<CompanyClaimResponse> RegisterAsync(
        this CompanyClaimClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateCompanyClaimRequest
        {
            AppId = appId,
            Key = key ?? Faker.Random.String2(5),
            Name = name ?? Faker.Random.String2(10),
        };
        var claimId = await client
            .CreateCompanyClaimAsync(request, Result.New(Guid.Empty).Error("Failed to create claim"))
            .GetDataAsync();
        var claims = await client
            .ListCompanyClaimsAsync(
                appId,
                Result.New(Array.Empty<CompanyClaimResponse>()).Error("Failed to list claims")
            )
            .GetDataAsync();

        return claims.Single(x => x.Id == claimId);
    }
}
