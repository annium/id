using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class RoleClientExtensions
{
    public static async Task<RoleResponse> RegisterAsync(
        this RoleClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateRoleRequest
        {
            AppId = appId,
            Key = key ?? Faker.Random.String2(5),
            Name = name ?? Faker.Random.String2(10),
        };
        var roleId = await client
            .CreateRoleAsync(request, Result.Create(Guid.Empty).Error("Failed to create role"))
            .GetDataAsync();
        var roles = await client
            .ListRolesAsync(appId, Result.Create(Array.Empty<RoleResponse>()).Error("Failed to list roles"))
            .GetDataAsync();

        return roles.Single(x => x.Id == roleId);
    }

    public static Task<IHttpResponse<IResult>> AddClaimToRoleAsync(
        this RoleClient client,
        Guid roleId,
        Guid claimId,
        string? value = null
    )
    {
        return client.AddClaimToRoleAsync(
            roleId,
            claimId,
            new AddClaimToRoleRequestBody { Value = value ?? Faker.Random.String2(10) },
            Result.Create().Error("Failed to add claim to role")
        );
    }
}
