using Annium.AspNetCore.IntegrationTesting;

namespace Annium.Id.Api.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest<Startup<Api.TestServicePack>>
    {
        public IntegrationTestBase()
        {
            Configure(request => request);
        }
    }
}