using Annium.Blazor.Net;
using Annium.Net.Http;

namespace Annium.Id.Site.Shared.Api.Server
{
    internal class ServerApi : IServerApi
    {
        public IHttpRequest Public => _requestFactory.New(_config.Server);

        private readonly IHttpRequestFactory _requestFactory;
        private readonly Configuration _config;

        public ServerApi(
            IHttpRequestFactory requestFactory,
            Configuration config
        )
        {
            _requestFactory = requestFactory;
            _config = config;
        }
    }

    public interface IServerApi : IApi
    {
        IHttpRequest Public { get; }
    }
}