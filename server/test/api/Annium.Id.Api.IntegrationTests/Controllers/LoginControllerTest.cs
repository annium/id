using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.TestClient;
using Annium.Id.Api.TestClient.Clients;
using Annium.Id.Api.ViewModels.Login.Requests;
using Annium.Testing;
using Xunit;

namespace Annium.Id.Api.IntegrationTests.Controllers
{
    public class LoginControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task LogIn_BadPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var request = new LogInRequest { Login = "uniqueLogin", Password = "Weak" };

            // act
            var response = await Id().Login.LogIn(app.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LogIn_InvalidLogin_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var request = new LogInRequest { Login = "uniqueLogin", Password = "StrongPass!!" };

            // act
            var response = await Id().Login.LogIn(app.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LogIn_InvalidPassword_Forbidden()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var request = new LogInRequest { Login = "demo", Password = "wrong_pass" };

            // act
            var response = await Id().Login.LogIn(app.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LogIn_Valid_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var request = new LogInRequest { Login = "demo", Password = "test1test" };

            // act
            var response = await Id().Login.LogIn(app.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateTokens_InvalidToken_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            token = await Id(token).LogUserIn(app.Id);

            // act
            var response = await Id(token).Login.UpdateToken(app.Id, Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateTokens_ValidToken_Ok()
        {
            // arrange
            var login = "demo";
            var email = "demo@demo";
            var password = "test1test";
            var token = await Id().RegisterLogUserIn(login, email, password);
            var app = await Id(token).App.Register();
            var tokens = await Id(token).Login.LogIn(app.Id, new LogInRequestBody { Login = login, Password = password }).GetData();

            // act
            var response = await Id(tokens.AccessToken).Login.UpdateToken(app.Id, tokens.RefreshToken).GetData();

            // assert
            response.AccessToken.IsNotDefault().IsNotEqual(tokens.AccessToken);
            response.RefreshToken.IsNotDefault().IsNotEqual(tokens.RefreshToken);
        }

        [Fact]
        public async Task LogOut_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            token = await Id(token).LogUserIn(app.Id);

            // act
            var response = await Id(token).Login.LogOut(app.Id);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}