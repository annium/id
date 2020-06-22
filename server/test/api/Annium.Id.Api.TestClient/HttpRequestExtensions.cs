using Annium.Net.Http;

namespace Annium.Id.Api.TestClient
{
    public static class HttpRequestExtensions
    {
        public static Client ApiClient(this IHttpRequest request) => new Client(request);
    }
}