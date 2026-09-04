using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.CompanyRoles;
using Server.ViewModels.Responses.CompanyRoles;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class CompanyRoleClientExtensions
{
    public static async Task<CompanyRoleResponse> RegisterAsync(
        this CompanyRoleClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateCompanyRoleRequest
        {
            AppId = appId,
            Key = key ?? Faker.Random.String2(5),
            Name = name ?? Faker.Random.String2(10),
        };
        var roleId = await client
            .CreateRoleAsync(request, Result.Create(Guid.Empty).Error("Failed to create role"))
            .GetDataAsync();
        var roles = await client
            .ListRolesAsync(appId, Result.Create(Array.Empty<CompanyRoleResponse>()).Error("Failed to list roles"))
            .GetDataAsync();

        return roles.Single(x => x.Id == roleId);
    }

    public static Task<IHttpResponse<IResult>> AddClaimToRoleAsync(
        this CompanyRoleClient client,
        Guid roleId,
        Guid claimId,
        string value = "Some"
    )
    {
        return client.AddClaimToRoleAsync(
            roleId,
            claimId,
            new AddCompanyClaimToCompanyRoleRequestBody { Value = value },
            Result.Create().Error("Failed to add claim to role")
        );
    }
}
