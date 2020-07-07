using System.Threading.Tasks;
using Annium.Id.Api.ViewModels.Apps.Requests;
using Annium.Id.Api.ViewModels.Apps.Responses;

namespace Annium.Id.Api.TestClient.Clients
{
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
    }
}