using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Core;
using Annium.Testing;
using Server.Email.Models;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Me;
using Server.ViewModels.Responses.Me;
using Xunit;
using Xunit.Abstractions;

namespace Server.IntegrationTests.Controllers;

public class MeControllerTest : IntegrationTestBase
{
    public MeControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task RegisterMe_IncorrectPayload_BadRequest()
    {
        // arrange
        var request = new RegisterMeRequest { Login = Faker.Internet.Email() };

        // act
        var response = await Id().Me.RegisterMe(request, Result.New().Error("Failed to register me"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterMe_LoginIsNotUnique_BadRequest()
    {
        // arrange
        var user = await Id().RegisterLogInGetUser();
        var request = new RegisterMeRequest
        {
            Server = Faker.Internet.Url(),
            Login = user.Login,
            Email = Faker.Internet.Email()
        };

        // act
        var response = await Id().Me.RegisterMe(request, Result.New().Error("Failed to register me"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterMe_EmailIsNotUnique_BadRequest()
    {
        // arrange
        var user = await Id().RegisterLogInGetUser();
        var request = new RegisterMeRequest
        {
            Server = Faker.Internet.Url(),
            Login = Faker.Internet.UserName(),
            Email = user.Email
        };

        // act
        var response = await Id().Me.RegisterMe(request, Result.New().Error("Failed to register me"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterMe_ReferralMissing_NotFound()
    {
        // arrange
        var request = new RegisterMeRequest
        {
            Server = Faker.Internet.Url(),
            Login = Faker.Internet.UserName(),
            Email = Faker.Internet.Email(),
            ReferralId = Guid.NewGuid()
        };

        // act
        var response = await Id().Me.RegisterMe(request, Result.New().Error("Failed to register me"));

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RegisterMe_ValidData_Ok()
    {
        // arrange
        var referral = await Id().RegisterLogInGetUser();
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();

        // act
        var response = await Id().RegisterLogInGetUser(login, email, password, referral.Id);

        // assert
        response.Id.IsNotDefault();
        response.Login.Is(login);
        response.Email.Is(email);
        response.ReferralId.Is(referral.Id);
    }

    [Fact]
    public async Task RestoreMyAccess_ValidEmail_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token)
            .Me.GetMe(Result.New(new MeResponse()).Error("Failed to load personal information"))
            .GetData();
        var request = new RestoreMyAccessRequestBody { Server = Faker.Internet.Url(), Email = user.Email };

        // act
        await Id(token)
            .Me.RestoreMyAccess(Constants.IdAppId, request, Result.New().Error("Failed to run access restore"));
        token = EmailService.Emails.Last().Data.As<RestoreAccessData>().Tokens.AccessToken;
        var response = await Id(token)
            .Me.GetMe(Result.New(new MeResponse()).Error("Failed to load personal information"))
            .GetData();

        // assert
        response.Id.Is(user.Id);
    }

    [Fact]
    public async Task GetMe_AuthenticatedUser_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token)
            .Me.GetMe(Result.New(new MeResponse()).Error("Failed to load personal information"))
            .GetData();

        // assert
        response.Id.IsNotDefault();
    }

    [Fact]
    public async Task GetMyToken_AuthenticatedUser_Ok()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var me = await Id(token)
            .Me.GetMe(Result.New(new MeResponse()).Error("Failed to load personal information"))
            .GetData();
        var app = await Id(token).App.Register();
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Id(token)
            .Me.GetMyToken(Result.New(new IdTokenResponse()).Error("Failed to load access tokens"));
        var idToken = response.Data.Data;

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        idToken.UserId.Is(me.Id);
        idToken.LoginId.IsNotDefault();
        idToken.App.Id.Is(app.Id);
    }

    [Fact]
    public async Task UpdateMyProfile_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateMyProfileRequest { Login = Faker.Internet.UserName() };

        // act
        var response = await Id(token).Me.UpdateMyProfile(request, Result.New().Error("Failed to update profile"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateMyProfile_LoginIsNotUnique_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var other = await Id().RegisterLogInGetUser();
        var request = new UpdateMyProfileRequest { Login = other.Login, Email = Faker.Internet.Email() };

        // act
        var response = await Id(token).Me.UpdateMyProfile(request, Result.New().Error("Failed to update profile"));

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateMyProfile_EmailIsNotUnique_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var other = await Id().RegisterLogInGetUser();
        var request = new UpdateMyProfileRequest { Login = Faker.Internet.UserName(), Email = other.Email };

        // act
        var response = await Id(token).Me.UpdateMyProfile(request, Result.New().Error("Failed to update profile"));

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateMyProfile_ValidData_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateMyProfileRequest { Login = Faker.Internet.UserName(), Email = Faker.Internet.Email() };

        // act
        var response = await Id(token).Me.UpdateMyProfile(request, Result.New().Error("Failed to update profile"));

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateMyPassword_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateMyPasswordRequest { Password = Faker.Internet.Password(4) };

        // act
        var response = await Id(token).Me.UpdateMyPassword(request, Result.New().Error("Failed to update password"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateMyPassword_ValidData_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateMyPasswordRequest { Password = Faker.Internet.Password() };

        // act
        var response = await Id(token).Me.UpdateMyPassword(request, Result.New().Error("Failed to update password"));

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UnregisterMe_ValidData_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Me.UnregisterMe(Result.New().Error("Failed to unregister"));

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
