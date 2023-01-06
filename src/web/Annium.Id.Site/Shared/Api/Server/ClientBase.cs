using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server;

public abstract class ClientBase
{
    protected IHttpRequest Request { get; }

    protected ClientBase(IHttpRequest request)
    {
        Request = request;
    }
}