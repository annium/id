using Annium.Net.Http;

namespace Annium.Id.Demo.TestClient
{
    public class Root
    {
        public IndexClient Index { get; }

        public Root(IHttpRequest request)
        {
            Index = new IndexClient(request);
        }
    }
}