using System;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.TestClient;

public static class CompanyClaimClientExtensions
{
    public static async Task<CompanyClaimResponse> Register(
        this CompanyClaimClient client,
        Guid appId,
        string key = "one",
        string name = "First claim"
    )
    {
        var request = new CreateCompanyClaimRequest { AppId = appId, Key = key, Name = name };
        var roleId = await client.CreateCompanyClaim(request).GetData();
        var roles = await client.ListCompanyClaims(appId).GetData();

        return roles.Single(x => x.Id == roleId);
    }

    public static Task<CompanyClaimResponse> RegisterOther(
        this CompanyClaimClient client,
        Guid appId,
        string key = "two",
        string name = "Second claim"
    )
    {
        return client.Register(appId, key, name);
    }
}