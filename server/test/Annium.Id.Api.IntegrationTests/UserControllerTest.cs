using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    [Skip]
    public class UserControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task GetUserById_ReturnsNotFoundIfUserIsMissing()
        {
            // arrange
            var userId = Guid.NewGuid();

            // act
            var response = await http.Get($"/users/{userId}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetUserById_ReturnsUserIfUserIsFound()
        {
            // arrange
            var user = await RegisterAsync();

            // act
            var response = await http.Get($"/users/{user.Id}").AsAsync<UserView>();

            // assert
            response.Id.IsEqual(user.Id);
        }
    }
}