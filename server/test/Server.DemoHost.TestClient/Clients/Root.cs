using Annium.Net.Http;

namespace Server.DemoHost.TestClient.Clients;

public class Root
{
    public IndexClient Index { get; }

    public Root(IHttpRequest request)
    {
        Index = new IndexClient(request);
    }
}