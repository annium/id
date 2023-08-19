using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class UserClientExtensions
{
    public static async Task<IHttpResponse<IResult>> AddUserClaim(
        this UserClient client,
        Guid userId,
        Guid claimId,
        string? value = null
    )
    {
        var response = await client.AddClaimToUser(userId, claimId, new AddClaimToUserRequestBody { Value = value ?? Faker.Random.String2(10) });

        return response;
    }
}