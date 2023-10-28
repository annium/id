using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class AppClientExtensions
{
    public static async Task<AppResponse> Register(this AppClient client, string? name = null)
    {
        var createResponse = await client.CreateApp(
            new CreateAppRequest { Name = name ?? Faker.Random.String2(10) },
            Result.New(Guid.Empty).Error("Failed to create app")
        );

        var getResponse = await client.GetApp(
            createResponse.Data.Data,
            Result.New(new AppResponse()).Error("Failed to load app")
        );

        return getResponse.Data.Data;
    }
}
