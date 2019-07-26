using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
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
            var payload = new UserLoginPayload { Login = "uniquelogin", Password = "asda" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_InvalidLogin_Forbidden()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new UserLoginPayload { Login = "uniquelogin", Password = "asdaasda" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_Forbidden()
        {
            // arrange
            var user = await RegisterUserAsync();
            var payload = new UserLoginPayload { Login = "demo", Password = "asdaasda" };

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
            var payload = new UserLoginPayload { Login = "demo", Password = "testtest" };

            // act
            var response = await id.Post("/me/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Logout_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Post("/me/logout").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateToken_InvalidToken_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Post("/me/token").Param("refreshToken", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateToken_ValidToken_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Post("/me/token").Param("refreshToken", tokens.RefreshToken).AsAsync<UserTokenView>();

            // assert
            response.AccessToken.IsNotDefault();
            response.RefreshToken.IsNotDefault();
        }

        [Fact]
        public async Task LoginAppAsync_BadPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new UserLoginPayload { Login = "uniquelogin", Password = "asda" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAppAsync_InvalidLogin_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new UserLoginPayload { Login = "uniquelogin", Password = "asdaasda" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAppAsync_InvalidPassword_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new UserLoginPayload { Login = "demo", Password = "asdaasda" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LoginAppAsync_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new UserLoginPayload { Login = "demo", Password = "testtest" };

            // act
            var response = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task LogoutApp_Ok()
        {
            // arrange
            var(user, app, tokens) = await LoginUserCreateAppLoginAppAsync();

            // act
            var response = await id.Post($"/me/apps/{app.Id}/logout").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateAppToken_InvalidToken_Forbidden()
        {
            // arrange
            var(user, app, tokens) = await LoginUserCreateAppLoginAppAsync();

            // act
            var response = await id.Post($"/me/apps/{app.Id}/token").Param("refreshToken", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateAppToken_ValidToken_Ok()
        {
            // arrange
            var(user, app, tokens) = await LoginUserCreateAppLoginAppAsync();

            // act
            var response = await id.Post($"/me/apps/{app.Id}/token").Param("refreshToken", tokens.RefreshToken).AsAsync<UserTokenView>();

            // assert
            response.AccessToken.IsNotDefault();
            response.RefreshToken.IsNotDefault();
        }
    }
}