using Annium.Net.Http;

namespace Annium.Id.Api.TestClient
{
    public static class HttpRequestExtensions
    {
        public static Client Client(this IHttpRequest request) => new Client(request);
    }
}