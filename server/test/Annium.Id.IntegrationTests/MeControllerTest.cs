using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.IntegrationTests
{
    public class MeControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Register_IncorrectPayload_BadRequest()
        {
            // arrange
            var payload = new UserPayload { Login = "demo" };

            // act
            var response = await id.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_LoginIsNotUnique_Conflict()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new UserPayload { Login = user.Login, Password = "asdasdsdd", Email = "asd1@demo.com" };

            // act
            var response = await id.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Register_EmailIsNotUnique_Conflict()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", Email = user.Email };

            // act
            var response = await id.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Register_ValidData_Ok()
        {
            // arrange
            var login = "demo";
            var password = "testtest";
            var email = "demo@demo.com";

            // act
            var response = await RegisterUserAsync(login, password, email);

            // assert
            response.Id.IsNotDefault();
            response.Login.IsEqual(login);
            response.Email.IsEqual(email);
        }

        [Fact]
        public async Task GetUser_AuthenticatedUser_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Get("/me").BearerAuthorization(tokens.AccessToken).AsAsync<UserPrivateView>();

            // assert
            response.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var payload = new UserPayload { Login = "demo" };

            // act
            var response = await id.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_LoginIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var other = await RegisterUserAsync("uniquelogin", "asdasdsdd", "asd1@demo.com");
            var update = new UserPayload { Login = other.Login, Password = "a96as9da", Email = "asd2@demo.com" };

            // act
            var response = await id.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(update).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_EmailIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var other = await RegisterUserAsync("uniquelogin", "asdasdsdd", "asd1@demo.com");
            var update = new UserPayload { Login = "otherlogin", Password = "a96as9da", Email = other.Email };

            // act
            var response = await id.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(update).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var payload = new UserPayload { Login = "medo", Password = "setsetset", Email = "medo@medo.com" };

            // act
            var response = await id.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(payload).AsAsync<UserPrivateView>();

            // assert
            response.Id.IsEqual(user.Id);
            response.Login.IsEqual(payload.Login);
            response.Email.IsEqual(payload.Email);
        }

        [Fact]
        public async Task Unregister_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Delete("/me").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}