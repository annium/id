using System.Net;
using System.Threading.Tasks;
using Annium.Id.ViewModels.Me.Requests;
using Annium.Id.ViewModels.Me.Responses;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class MeControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Register_IncorrectPayload_BadRequest()
        {
            // arrange
            var payload = new RegisterMeRequest { Login = "demo" };

            // act
            var response = await id.Post("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_LoginIsNotUnique_BadRequest()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new RegisterMeRequest { Login = user.Login, Password = "asdasdsdd", Email = "asd1@demo.com" };

            // act
            var response = await id.Post("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_EmailIsNotUnique_BadRequest()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new RegisterMeRequest { Login = "uniquelogin", Password = "asdasdsdd", Email = user.Email };

            // act
            var response = await id.Post("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
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
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Get("/me").BearerAuthorization(tokens.AccessToken).AsResultAsync<MeResponse>();

            // assert
            response.Data.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var payload = new RegisterMeRequest { Login = "demo" };

            // act
            var response = await id.Put("/me").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_LoginIsNotUnique_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var other = await RegisterUserAsync("uniquelogin", "asdasdsdd", "asd1@demo.com");
            var update = new UpdateMeRequest { Login = other.Login, Password = "a96as9da", Email = "asd2@demo.com" };

            // act
            var response = await id.Put("/me").BearerAuthorization(tokens.AccessToken).JsonContent(update).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_EmailIsNotUnique_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var other = await RegisterUserAsync("uniquelogin", "asdasdsdd", "asd1@demo.com");
            var update = new UpdateMeRequest { Login = "otherlogin", Password = "a96as9da", Email = other.Email };

            // act
            var response = await id.Put("/me").BearerAuthorization(tokens.AccessToken).JsonContent(update).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var payload = new RegisterMeRequest { Login = "medo", Password = "setsetset", Email = "medo@medo.com" };

            // act
            var response = await id.Put("/me").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Unregister_ValidData_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete("/me").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}