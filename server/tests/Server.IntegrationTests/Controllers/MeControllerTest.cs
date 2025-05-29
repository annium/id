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
        var response = await Id()
            .Me.RegisterMeAsync(
                request,
                Result.New().Error("Failed to register me"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterMe_LoginIsNotUnique_BadRequest()
    {
        // arrange
        var user = await Id().RegisterLogInGetUserAsync();
        var request = new RegisterMeRequest
        {
            Server = Faker.Internet.Url(),
            Login = user.Login,
            Email = Faker.Internet.Email(),
        };

        // act
        var response = await Id()
            .Me.RegisterMeAsync(
                request,
                Result.New().Error("Failed to register me"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterMe_EmailIsNotUnique_BadRequest()
    {
        // arrange
        var user = await Id().RegisterLogInGetUserAsync();
        var request = new RegisterMeRequest
        {
            Server = Faker.Internet.Url(),
            Login = Faker.Internet.UserName(),
            Email = user.Email,
        };

        // act
        var response = await Id()
            .Me.RegisterMeAsync(
                request,
                Result.New().Error("Failed to register me"),
                TestContext.Current.CancellationToken
            );

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
            ReferralId = Guid.NewGuid(),
        };

        // act
        var response = await Id()
            .Me.RegisterMeAsync(
                request,
                Result.New().Error("Failed to register me"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RegisterMe_ValidData_Ok()
    {
        // arrange
        var referral = await Id().RegisterLogInGetUserAsync();
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();

        // act
        var response = await Id().RegisterLogInGetUserAsync(login, email, password, referral.Id);

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
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var request = new RestoreMyAccessRequestBody { Server = Faker.Internet.Url(), Email = user.Email };

        // act
        await Id(token)
            .Me.RestoreMyAccessAsync(
                Constants.IdAppId,
                request,
                Result.New().Error("Failed to run access restore"),
                TestContext.Current.CancellationToken
            );
        token = EmailService.Emails.Last().Data.As<RestoreAccessData>().Tokens.AccessToken;
        var response = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.Id.Is(user.Id);
    }

    [Fact]
    public async Task GetMe_AuthenticatedUser_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var me = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(token).App.RegisterAsync();
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var response = await Id(token)
            .Me.GetMyTokenAsync(
                Result.New(new IdTokenResponse()).Error("Failed to load access tokens"),
                TestContext.Current.CancellationToken
            );
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
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateMyProfileRequest { Login = Faker.Internet.UserName() };

        // act
        var response = await Id(token)
            .Me.UpdateMyProfileAsync(
                request,
                Result.New().Error("Failed to update profile"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateMyProfile_LoginIsNotUnique_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var other = await Id().RegisterLogInGetUserAsync();
        var request = new UpdateMyProfileRequest { Login = other.Login, Email = Faker.Internet.Email() };

        // act
        var response = await Id(token)
            .Me.UpdateMyProfileAsync(
                request,
                Result.New().Error("Failed to update profile"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateMyProfile_EmailIsNotUnique_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var other = await Id().RegisterLogInGetUserAsync();
        var request = new UpdateMyProfileRequest { Login = Faker.Internet.UserName(), Email = other.Email };

        // act
        var response = await Id(token)
            .Me.UpdateMyProfileAsync(
                request,
                Result.New().Error("Failed to update profile"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task UpdateMyProfile_ValidData_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateMyProfileRequest { Login = Faker.Internet.UserName(), Email = Faker.Internet.Email() };

        // act
        var response = await Id(token)
            .Me.UpdateMyProfileAsync(
                request,
                Result.New().Error("Failed to update profile"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateMyPassword_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateMyPasswordRequest { Password = Faker.Internet.Password(4) };

        // act
        var response = await Id(token)
            .Me.UpdateMyPasswordAsync(
                request,
                Result.New().Error("Failed to update password"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateMyPassword_ValidData_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateMyPasswordRequest { Password = Faker.Internet.Password() };

        // act
        var response = await Id(token)
            .Me.UpdateMyPasswordAsync(
                request,
                Result.New().Error("Failed to update password"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UnregisterMe_ValidData_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Me.UnregisterMeAsync(Result.New().Error("Failed to unregister"), TestContext.Current.CancellationToken);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
