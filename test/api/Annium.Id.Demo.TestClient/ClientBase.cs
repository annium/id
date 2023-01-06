using Annium.Net.Http;

namespace Annium.Id.Demo.TestClient;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}