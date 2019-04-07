using System;
using System.Net;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Extensions.Net.Http;
using Annium.IdentityServer.Db;
using Annium.IdentityServer.Payloads;
using Annium.Testing;
using Microsoft.EntityFrameworkCore;

namespace Annium.IdentityServer.IntegrationTests
{
    public class AppControllerTest : IntegrationTest<Startup<TestServicePack>>
    {
        public AppControllerTest()
        {
            Configure(request => request);
        }

        [Fact]
        public async Task ListAsync_Works()
        {
            // act
            var response = await http.Get("/app").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).IsEqual("[]");
        }

        [Fact]
        public async Task CreateAsync_Works()
        {
            // arrange
            var payload = new AppPayload() { Login = "demo", Password = "demodemode", Name = "demo", Email = "demo@demo.xx" };

            // act
            var app = await http.Put("/app").JsonContent(payload).AsAsync<App>();
            var apps = await http.Get("/app").AsAsync<App[]>();

            // assert
            apps.Has(1);
            app.Login.IsEqual(payload.Login);
            app.PasswordHash.IsNotDefault().IsNotEqual(payload.Password);
            app.ApiToken.IsNotDefault();
            app.Name.IsEqual(payload.Name);
            app.Email.IsEqual(payload.Email);
        }

        [Fact]
        public async Task UpdateAsync_Works()
        {
            // arrange
            var id = Guid.NewGuid();

            // act
            var response = await http.Post($"/app/{id}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_Works()
        {
            // arrange
            var id = Guid.NewGuid();

            // act
            var response = await http.Delete($"/app/{id}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}