using Annium.Net.Http;

namespace Server.DemoHost.TestClient.Clients;

public class Root
{
    public IndexClient Index => new(_request);
    private readonly IHttpRequest _request;

    internal Root(IHttpRequest request)
    {
        _request = request;
    }
}
