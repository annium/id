using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Email.Models;
using Annium.Id.ViewModels.Me.Requests;
using Annium.Id.ViewModels.Me.Responses;
using Annium.Net.Http;
using Annium.Testing;
using Xunit;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class MeControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task RegisterMe_IncorrectPayload_BadRequest()
        {
            // arrange
            var payload = new RegisterMeRequest { Login = "demo" };

            // act
            var response = await id.Post("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterMe_LoginIsNotUnique_BadRequest()
        {
            // arrange
            var user = await RegisterLogUserInGetUserAsync();
            var payload = new RegisterMeRequest { Server = "http://localhost/", Login = user.Login, Email = "asd1@demo.com" };

            // act
            var response = await id.Post("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterMe_EmailIsNotUnique_BadRequest()
        {
            // arrange
            var user = await RegisterLogUserInGetUserAsync();
            var payload = new RegisterMeRequest { Server = "http://localhost/", Login = "uniquelogin", Email = user.Email };

            // act
            var response = await id.Post("/me").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterMe_ValidData_Ok()
        {
            // arrange
            var login = "demo";
            var password = "testtest";
            var email = "demo@demo.com";

            // act
            var response = await RegisterLogUserInGetUserAsync(login, password, email);

            // assert
            response.Id.IsNotDefault();
            response.Login.IsEqual(login);
            response.Email.IsEqual(email);
        }

        [Fact]
        public async Task RestoreMyAccess_ValidEmail_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync(email: "someemail@email.com");
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            await id.Post($"/me/{Constants.IdAppId}/restore-access")
                .JsonContent(new RestoreMyAccessRequestBase { Server = "http://localhost/", Email = "someemail@email.com" })
                .EnsureSuccessStatusCode()
                .RunAsync();
            var accessToken = emailService.Emails.Last().Data.As<RestoreAccessData>().Tokens.AccessToken;
            var response = await id.Get("/me").BearerAuthorization(accessToken).AsResultAsync<MeResponse>();

            // assert
            response.Data.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task GetMe_AuthenticatedUser_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            var response = await id.Get("/me").BearerAuthorization(tokens.AccessToken).AsResultAsync<MeResponse>();

            // assert
            response.Data.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task UpdateMyProfile_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new UpdateMyProfileRequest { Login = "demo" };

            // act
            var response = await id.Put("/me/profile").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateMyProfile_LoginIsNotUnique_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var other = await RegisterLogUserInGetUserAsync("uniquelogin", "asdasdsdd", "asd1@demo.com");
            var update = new UpdateMyProfileRequest { Login = other.Login, Email = "asd2@demo.com" };

            // act
            var response = await id.Put("/me/profile").BearerAuthorization(tokens.AccessToken).JsonContent(update).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateMyProfile_EmailIsNotUnique_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var other = await RegisterLogUserInGetUserAsync("uniquelogin", "asdasdsdd", "asd1@demo.com");
            var update = new UpdateMyProfileRequest { Login = "otherlogin", Email = other.Email };

            // act
            var response = await id.Put("/me/profile").BearerAuthorization(tokens.AccessToken).JsonContent(update).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateMyProfile_ValidData_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new UpdateMyProfileRequest { Login = "medo", Email = "medo@medo.com" };

            // act
            var response = await id.Put("/me/profile").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateMyPassword_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new UpdateMyPasswordRequest { Password = "demo" };

            // act
            var response = await id.Put("/me/password").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateMyPassword_ValidData_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new UpdateMyPasswordRequest { Password = "setsetset" };

            // act
            var response = await id.Put("/me/password").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UnregisterMe_ValidData_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete("/me").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}