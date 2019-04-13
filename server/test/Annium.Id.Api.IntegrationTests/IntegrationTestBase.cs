using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;

namespace Annium.Id.Api.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest<Startup<Api.TestServicePack>>
    {
        protected UserTokenView tokens;

        public IntegrationTestBase()
        {
            Configure(request => request);
        }

        protected async Task<UserView> CreateTestUserAsync()
        {
            var payload = new UserPayload { Login = "demo", Password = "testtest", FirstName = "demo", LastName = "medo", Email = "demo@demo.com" };

            return await http.Put("/me").JsonContent(payload).AsAsync<UserView>();
        }

        protected async Task<UserView> LoginAsync()
        {
            var user = await CreateTestUserAsync();
            var payload = new UserLoginPayload { Login = user.Login, Password = "testtest" };

            tokens = await http.Post("/me/login").JsonContent(payload).AsAsync<UserTokenView>();

            return user;
        }
    }
}