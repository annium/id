using Annium.Net.Http;

namespace Site.Shared.Api.Server.Clients;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}