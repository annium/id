using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.ViewModels.Login.Requests;
using Annium.Id.ViewModels.Login.Responses;
using Annium.Net.Http;
using Annium.Testing;
using Xunit;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class LoginControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task LogIn_BadPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogInRequest { Login = "uniquelogin", Password = "asda" };

            // act
            var response = await id.Post($"/me/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LogIn_InvalidLogin_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogInRequest { Login = "uniquelogin", Password = "asdaasda" };

            // act
            var response = await id.Post($"/me/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LogIn_InvalidPassword_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogInRequest { Login = "demo", Password = "asdaasda" };

            // act
            var response = await id.Post($"/me/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LogIn_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new LogInRequest { Login = "demo", Password = "testtest" };

            // act
            var response = await id.Post($"/me/{app.Id}/login").JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateTokens_InvalidToken_NotFound()
        {
            // arrange
            var (_, app, _) = await RegisterLogUserInCreateAppLogInAppAsync();
            var tokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await id.Put($"/me/{app.Id}/token")
                .BearerAuthorization(tokens.AccessToken)
                .Param("refreshToken", Guid.NewGuid())
                .RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateTokens_ValidToken_Ok()
        {
            // arrange
            var (_, app, _) = await RegisterLogUserInCreateAppLogInAppAsync();
            var tokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await id.Put($"/me/{app.Id}/token")
                .BearerAuthorization(tokens.AccessToken)
                .Param("refreshToken", tokens.RefreshToken)
                .AsResultAsync<TokensResponse>();

            // assert
            response.Data.AccessToken.IsNotDefault().IsNotEqual(tokens.AccessToken);
            response.Data.RefreshToken.IsNotDefault().IsNotEqual(tokens.RefreshToken);
        }

        [Fact]
        public async Task LogOut_Ok()
        {
            // arrange
            var (_, app, _) = await RegisterLogUserInCreateAppLogInAppAsync();
            var tokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await id.Delete($"/me/{app.Id}/logout").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}