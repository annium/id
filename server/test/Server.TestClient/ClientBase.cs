using Annium.Net.Http;

namespace Server.TestClient;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}