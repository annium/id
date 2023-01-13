using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Net.Http;
using Server.ViewModels.Requests.Users;

namespace Server.TestClient;

public static class UserClientExtensions
{
    public static async Task<IHttpResponse<IResult>> AddUserClaim(
        this UserClient client,
        Guid userId,
        Guid claimId,
        string value = "Some"
    )
    {
        var response = await client.AddClaimToUser(claimId, userId, new AddClaimToUserRequestBody { Value = value });

        return response;
    }
}