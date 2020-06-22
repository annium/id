using Annium.Net.Http;

namespace Annium.Id.Demo.TestClient
{
    public static class HttpRequestExtensions
    {
        public static Client DemoClient(this IHttpRequest request) => new Client(request);
    }
}