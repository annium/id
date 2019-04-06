using System;
using System.Net;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Extensions.Net.Http;
using Annium.Testing;

namespace Annium.IdentityServer.IntegrationTests
{
    public class AppControllerTest : IntegrationTest<Startup<ServicePack>>
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
        }

        [Fact]
        public async Task CreateAsync_Works()
        {
            // act
            var response = await http.Put("/app").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
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