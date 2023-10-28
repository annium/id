using Annium.Net.Http;

namespace Server.DemoHost.TestClient.Clients;

public static class HttpRequestExtensions
{
    public static DemoClient DemoClient(this IHttpRequest request) => new(request);
}
