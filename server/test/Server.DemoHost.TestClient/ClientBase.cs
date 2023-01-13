using Annium.Net.Http;

namespace Server.DemoHost.TestClient;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}