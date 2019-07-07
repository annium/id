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
            var user = await RegisterAsync();
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
            var user = await RegisterAsync();
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
            var payload = new UserPayload { Login = "demo", Password = "testtest", Email = "demo@demo.com" };

            // act
            var response = await id.Put("/me").JsonContent(payload).AsAsync<UserPrivateView>();

            // assert
            response.Id.IsNotDefault();
            response.Login.IsEqual(payload.Login);
            response.Email.IsEqual(payload.Email);
        }

        [Fact]
        public async Task GetUser_AuthenticatedUser_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Get("/me").BearerAuthorization(tokens.AccessToken).AsAsync<UserPrivateView>();

            // assert
            response.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
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
            var(user, tokens) = await LoginAsync();
            var p = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", Email = "asd1@demo.com" };
            await id.Put("/me").JsonContent(p).RunAsync();
            var u = new UserPayload { Login = p.Login, Password = "a96as9da", Email = "asd2@demo.com" };

            // act
            var response = await id.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_EmailIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", Email = "asd1@demo.com" };
            await id.Put("/me").JsonContent(p).RunAsync();
            var u = new UserPayload { Login = "otherlogin", Password = "a96as9da", Email = p.Email };

            // act
            var response = await id.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
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
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete("/me").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}