using System.Threading.Tasks;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;
using static Server.Host.TestClient.Helper;

namespace Server.Host.TestClient;

public static class AppClientExtensions
{
    public static async Task<AppResponse> Register(
        this AppClient client,
        string? name = null
    )
    {
        var createResponse = await client.CreateApp(new CreateAppRequest { Name = name ?? Faker.Random.String2(10) });

        var getResponse = await client.GetApp(createResponse.Data.Data);

        return getResponse.Data.Data;
    }
}