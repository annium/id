using Annium.Net.Http;

namespace Server.DemoHost.TestClient;

public class Client
{
    public IndexClient Index { get; }

    public Client(IHttpRequest request)
    {
        Index = new IndexClient(request);
    }
}