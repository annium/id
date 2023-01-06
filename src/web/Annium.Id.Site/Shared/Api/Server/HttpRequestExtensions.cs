using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server;

public static class HttpRequestExtensions
{
    public static Client Client(this IHttpRequest request) => new Client(request);
}