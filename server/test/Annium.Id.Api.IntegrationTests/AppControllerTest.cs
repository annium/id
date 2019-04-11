using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Db;
using Annium.Testing;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Api.IntegrationTests
{
    public class AppControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task ListAsync_Works()
        {
            // act
            var response = await http.Get("/apps").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).IsEqual("[]");
        }

        // [Fact]
        // public async Task CreateAsync_Works()
        // {
        //     // arrange
        //     var payload = new AppPayload() { Key = "demo", Name = "Demo App" };

        //     // act
        //     var app = await http.Put("/apps").JsonContent(payload).AsAsync<App>();
        //     var apps = await http.Get("/apps").AsAsync<App[]>();

        //     // assert
        //     apps.Has(1);
        //     app.Key.IsEqual(payload.Key);
        //     app.Name.IsEqual(payload.Name);
        //     app.ApiToken.IsNotDefault();
        // }

        [Fact]
        public async Task UpdateAsync_Works()
        {
            // arrange
            var id = Guid.NewGuid();

            // act
            var response = await http.Post($"/apps/{id}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_Works()
        {
            // arrange
            var id = Guid.NewGuid();

            // act
            var response = await http.Delete($"/apps/{id}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}