using Annium.Net.Http;

namespace Annium.Id.Api.TestClient;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}