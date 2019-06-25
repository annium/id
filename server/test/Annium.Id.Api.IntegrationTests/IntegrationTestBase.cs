using System;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;

namespace Annium.Id.Api.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest<Startup<Api.TestServicePack>>
    {
        public IntegrationTestBase()
        {
            Configure(request => request);
        }

        protected async Task<UserView> RegisterAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var payload = new UserPayload { Login = login, Password = password, Email = email };

            return await http.Put("/me").JsonContent(payload).AsAsync<UserView>();
        }

        protected async Task<ValueTuple<UserView, UserTokenView>> LoginAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var user = await RegisterAsync(login, password, email);
            var payload = new UserLoginPayload { Login = login, Password = password };

            var tokens = await http.Post("/me/login").JsonContent(payload).AsAsync<UserTokenView>();

            return (user, tokens);
        }

        protected Task<AppView> CreateAppAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo App"
        )
        {
            var payload = new AppPayload { Key = key, Name = name };

            return http.Put("/apps").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<AppView>();
        }

        protected Task<CompanyView> CreateCompanyAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo App",
            Guid? parentId = null
        )
        {
            var payload = new CompanyPayload { ParentId = parentId, Key = key, Name = name };

            return http.Put("/companies").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<CompanyView>();
        }
    }
}