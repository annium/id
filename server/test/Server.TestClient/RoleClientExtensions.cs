using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;
using static Server.TestClient.Helper;

namespace Server.TestClient;

public static class RoleClientExtensions
{
    public static async Task<RoleResponse> Register(
        this RoleClient client,
        Guid appId,
        string? key = null,
        string? name = null
    )
    {
        var request = new CreateRoleRequest { AppId = appId, Key = key ?? Faker.Random.String(5), Name = name ?? Faker.Random.String(10) };
        var roleId = await client.CreateRole(request).GetData();
        var roles = await client.ListRoles(appId).GetData();

        return roles.Single(x => x.Id == roleId);
    }

    public static Task<IHttpResponse<IResult>> AddClaimToRole(
        this RoleClient client,
        Guid claimId,
        Guid roleId,
        string? value = null
    )
    {
        return client.AddClaimToRole(claimId, roleId, new AddClaimToRoleRequestBody { Value = value ?? Faker.Random.String(10) });
    }
}