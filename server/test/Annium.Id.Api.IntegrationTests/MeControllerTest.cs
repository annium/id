using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    public class MeControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Register_IncorrectPayload_BadRequest()
        {
            // arrange
            var payload = new UserPayload { Login = "demo" };

            // act
            var response = await http.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_LoginIsNotUnique_Conflict()
        {
            // arrange
            var user = await RegisterAsync();
            var payload = new UserPayload { Login = user.Login, Password = "asdasdsdd", FirstName = "a123", LastName = "adsudq", Email = "asd1@demo.com" };

            // act
            var response = await http.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Register_EmailIsNotUnique_Conflict()
        {
            // arrange
            var user = await RegisterAsync();
            var payload = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", FirstName = "a123", LastName = "adsudq", Email = user.Email };

            // act
            var response = await http.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Register_ValidData_Ok()
        {
            // arrange
            var payload = new UserPayload { Login = "demo", Password = "testtest", FirstName = "demo", LastName = "medo", Email = "demo@demo.com" };

            // act
            var response = await http.Put("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUser_AuthenticatedUser_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Get("/me").BearerAuthorization(tokens.AccessToken).AsAsync<UserView>();

            // assert
            response.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task LoginAsync_BadPayload_BadRequest()
        {
            // arrange
            var user = await RegisterAsync();
            var payload = new UserLoginPayload { Login = "uniquelogin", Password = "asda" };

            // act
            var response = await http.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_InvalidLogin_Forbidden()
        {
            // arrange
            var user = await RegisterAsync();
            var payload = new UserLoginPayload { Login = "uniquelogin", Password = "asdaasda" };

            // act
            var response = await http.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_Forbidden()
        {
            // arrange
            var user = await RegisterAsync();
            var payload = new UserLoginPayload { Login = "demo", Password = "asdaasda" };

            // act
            var response = await http.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAsync_Valid_Ok()
        {
            // arrange
            var user = await RegisterAsync();
            var payload = new UserLoginPayload { Login = "demo", Password = "testtest" };

            // act
            var response = await http.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Logout_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post("/me/logout").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateToken_InvalidToken_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post("/me/token").Param("refreshToken", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateToken_ValidToken_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post("/me/token").Param("refreshToken", tokens.RefreshToken).AsAsync<UserTokenView>();

            // assert
            response.AccessToken.IsNotDefault();
            response.RefreshToken.IsNotDefault();
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new UserPayload { Login = "demo" };

            // act
            var response = await http.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_LoginIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", FirstName = "a123", LastName = "adsudq", Email = "asd1@demo.com" };
            await http.Put("/me").JsonContent(p).RunAsync();
            var u = new UserPayload { Login = p.Login, Password = "a96as9da", FirstName = "a123", LastName = "adsudq", Email = "asd2@demo.com" };

            // act
            var response = await http.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_EmailIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new UserPayload { Login = "uniquelogin", Password = "asdasdsdd", FirstName = "a123", LastName = "adsudq", Email = "asd1@demo.com" };
            await http.Put("/me").JsonContent(p).RunAsync();
            var u = new UserPayload { Login = "otherlogin", Password = "a96as9da", FirstName = "a123", LastName = "adsudq", Email = p.Email };

            // act
            var response = await http.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new UserPayload { Login = "medo", Password = "setsetset", FirstName = "medo", LastName = "demo", Email = "medo@medo.com" };

            // act
            var response = await http.Post("/me").BearerAuthorization(tokens.AccessToken).JsonContent(payload).AsAsync<UserView>();

            // assert
            response.Id.IsEqual(user.Id);
            response.Login.IsEqual(payload.Login);
            response.FirstName.IsEqual(payload.FirstName);
            response.LastName.IsEqual(payload.LastName);
            response.Email.IsEqual(payload.Email);
        }

        [Fact]
        public async Task Unregister_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete("/me").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}