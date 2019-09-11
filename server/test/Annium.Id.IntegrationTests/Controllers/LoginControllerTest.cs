using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Id.ViewModels.Users.Responses;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class LoginControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task LoginAsync_BadPayload_BadRequest()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new LogUserInRequest { Login = "uniquelogin", Password = "asda" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_InvalidLogin_NotFound()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new LogUserInRequest { Login = "uniquelogin", Password = "asdaasda" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_Forbidden()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new LogUserInRequest { Login = "demo", Password = "asdaasda" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAsync_Valid_Ok()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new LogUserInRequest { Login = "demo", Password = "testtest" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Logout_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete("/me/logout").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateToken_InvalidToken_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Put("/me/token").Param("refreshToken", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateToken_ValidToken_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Put("/me/token").Param("refreshToken", tokens.RefreshToken).AsAsync<IResult<UserTokenResponse>>();

            // assert
            response.Data.AccessToken.IsNotDefault();
            response.Data.RefreshToken.IsNotDefault();
        }

        [Fact]
        public async Task LoginAppAsync_BadPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogUserInRequest { Login = "uniquelogin", Password = "asda" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAppAsync_InvalidLogin_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogUserInRequest { Login = "uniquelogin", Password = "asdaasda" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginAppAsync_InvalidPassword_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogUserInRequest { Login = "demo", Password = "asdaasda" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAppAsync_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogUserInRequest { Login = "demo", Password = "testtest" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task LogoutApp_Ok()
        {
            // arrange
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();

            // act
            var response = await id.Delete($"/me/apps/{app.Id}/logout").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAppToken_InvalidToken_NotFound()
        {
            // arrange
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();

            // act
            var response = await id.Put($"/me/apps/{app.Id}/token").Param("refreshToken", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateAppToken_ValidToken_Ok()
        {
            // arrange
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();

            // act
            var response = await id.Put($"/me/apps/{app.Id}/token").Param("refreshToken", tokens.RefreshToken).AsAsync<IResult<UserTokenResponse>>();

            // assert
            response.Data.AccessToken.IsNotDefault();
            response.Data.RefreshToken.IsNotDefault();
        }
    }
}