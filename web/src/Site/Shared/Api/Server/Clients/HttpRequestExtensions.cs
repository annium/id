using Annium.Net.Http;

namespace Site.Shared.Api.Server.Clients;

public static class HttpRequestExtensions
{
    public static Root Client(this IHttpRequest request) => new(request);
}