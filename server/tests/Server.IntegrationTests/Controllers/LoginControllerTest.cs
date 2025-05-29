using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Login;
using Server.ViewModels.Responses.Login;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class LoginControllerTest : IntegrationTestBase
{
    public LoginControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task LogIn_BadPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var request = new LogInRequest { Login = Faker.Internet.UserName(), Password = Faker.Internet.Password(4) };

        // act
        var response = await Id()
            .Login.LogInAsync(
                app.Id,
                request,
                Result.New(new TokensResponse()).Error("Failed to log in"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task LogIn_InvalidLogin_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var request = new LogInRequest { Login = Faker.Internet.UserName(), Password = Faker.Internet.Password() };

        // act
        var response = await Id()
            .Login.LogInAsync(
                app.Id,
                request,
                Result.New(new TokensResponse()).Error("Failed to log in"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task LogIn_InvalidPassword_Forbidden()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var token = await Id().RegisterLogUserInAsync(login: login);
        var app = await Id(token).App.RegisterAsync();
        var request = new LogInRequest { Login = login, Password = Faker.Internet.Password() };

        // act
        var response = await Id()
            .Login.LogInAsync(
                app.Id,
                request,
                Result.New(new TokensResponse()).Error("Failed to log in"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task LogIn_Valid_Ok()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserInAsync(login: login, password: password);
        var app = await Id(token).App.RegisterAsync();
        var request = new LogInRequest { Login = login, Password = password };

        // act
        var response = await Id()
            .Login.LogInAsync(
                app.Id,
                request,
                Result.New(new TokensResponse()).Error("Failed to log in"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateTokens_InvalidToken_NotFound()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var app = await Id(token).App.RegisterAsync();
        token = await Id(token).LogUserInAsync(app.Id, login, password);

        // act
        var response = await Id(token)
            .Login.UpdateTokenAsync(
                app.Id,
                Guid.NewGuid(),
                Result.New(new TokensResponse()).Error("Failed to update token"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTokens_ValidToken_Ok()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var app = await Id(token).App.RegisterAsync();
        var tokens = await Id(token)
            .Login.LogInAsync(
                app.Id,
                new LogInRequestBody { Login = login, Password = password },
                Result.New(new TokensResponse()).Error("Failed to log in"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(tokens.AccessToken)
            .Login.UpdateTokenAsync(
                app.Id,
                tokens.RefreshToken,
                Result.New(new TokensResponse()).Error("Failed to update token"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.AccessToken.IsNotDefault().IsNotEqual(tokens.AccessToken);
        response.RefreshToken.IsNotDefault().IsNotEqual(tokens.RefreshToken);
    }

    [Fact]
    public async Task LogOut_Ok()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var app = await Id(token).App.RegisterAsync();
        token = await Id(token).LogUserInAsync(app.Id, login, password);

        // act
        var response = await Id(token)
            .Login.LogOutAsync(app.Id, Result.New().Error("Failed to log out"), TestContext.Current.CancellationToken);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
