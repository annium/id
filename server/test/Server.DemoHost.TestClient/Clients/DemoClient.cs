using Annium.Net.Http;

namespace Server.DemoHost.TestClient.Clients;

public class DemoClient : Root
{
    public DemoClient(IHttpRequest request) : base(request)
    {
    }
}