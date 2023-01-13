using Annium.Net.Http;

namespace Server.DemoHost.TestClient;

public static class HttpRequestExtensions
{
    public static Client DemoClient(this IHttpRequest request)
    {
        return new Client(request);
    }
}