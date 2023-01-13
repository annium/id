using System.Threading.Tasks;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;

namespace Server.TestClient;

public static class AppClientExtensions
{
    public static async Task<AppResponse> Register(
        this AppClient client,
        string name = "Demo App"
    )
    {
        var createResponse = await client.CreateApp(new CreateAppRequest { Name = name });

        var getResponse = await client.GetApp(createResponse.Data.Data);

        return getResponse.Data.Data;
    }

    public static Task<AppResponse> RegisterOther(
        this AppClient client,
        string name = "Other App"
    )
    {
        return client.Register(name);
    }
}