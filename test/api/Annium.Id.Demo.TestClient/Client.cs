using Annium.Net.Http;

namespace Annium.Id.Demo.TestClient;

public class Client
{
    public IndexClient Index { get; }

    public Client(IHttpRequest request)
    {
        Index = new IndexClient(request);
    }
}