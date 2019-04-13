using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    public class UserControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task RegisterAsync_FailsWithIncorrectPayload()
        {
            // arrange
            var payload = new UserPayload { Login = "demo" };

            // act
            var response = await http.Put("/users").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterAsync_WorksWithValidData()
        {
            // arrange
            var payload = new UserPayload { Login = "demo", Password = "testtest", FirstName = "demo", LastName = "medo", Email = "demo@demo.com" };

            // act
            var response = await http.Put("/users").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterAsync_FailsIfLoginIsNotUnique()
        {
            // arrange
            var user = await CreateTestUserAsync();
            var payload = new UserPayload { Login = user.Login, Password = "asdasdsdd", FirstName = "a123", LastName = "adsudq", Email = "asd1@demo.com" };

            // act
            var response = await http.Put("/users").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task RegisterAsync_FailsIfEmailIsNotUnique()
        {
            // arrange
            var user = await CreateTestUserAsync();
            var payload = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", FirstName = "a123", LastName = "adsudq", Email = user.Email };

            // act
            var response = await http.Put("/users").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

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
            var user = await CreateTestUserAsync();

            // act
            var response = await http.Get($"/users/{user.Id}").AsAsync<UserView>();

            // assert
            response.Id.IsEqual(user.Id);
        }
    }
}