using Annium.Net.Http;

namespace Server.Host.TestClient.Clients;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}