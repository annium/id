using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class UserClientExtensions
{
    public static async Task<IHttpResponse<IResult>> AddUserClaimAsync(
        this UserClient client,
        Guid userId,
        Guid claimId,
        string? value = null
    )
    {
        var response = await client.AddClaimToUserAsync(
            userId,
            claimId,
            new AddClaimToUserRequestBody { Value = value ?? Faker.Random.String2(10) },
            Result.New().Error("Failed to add claim")
        );

        return response;
    }
}
