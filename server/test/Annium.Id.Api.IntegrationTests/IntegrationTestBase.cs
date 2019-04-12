using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Db;

namespace Annium.Id.Api.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest<Startup<Api.TestServicePack>>
    {
        public IntegrationTestBase()
        {
            Configure(request => request);
        }

        protected async Task<User> CreateTestUserAsync()
        {
            var payload = new UserPayload { Login = "demo", Password = "testtest", FirstName = "demo", LastName = "medo", Email = "demo@demo.com" };

            return await http.Put("/users").JsonContent(payload).AsAsync<User>();
        }
    }
}