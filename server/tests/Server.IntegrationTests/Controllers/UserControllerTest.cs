using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Models.Extensions;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Responses.Me;
using Server.ViewModels.Responses.Users;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class UserControllerTest : IntegrationTestBase
{
    public UserControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task FindUsers_Ok()
    {
        // arrange
        var me = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id()
            .User.FindUsersAsync(
                me.Login,
                1,
                Result.New(Array.Empty<UserResponse>()).Error("Failed to find users"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(
            new[]
            {
                new UserResponse { Id = me.Id, Login = me.Login },
            }
        );
    }

    [Fact]
    public async Task GetUser_Ok()
    {
        // arrange
        var me = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id()
            .User.GetUserAsync(
                me.Id,
                Result.New(new UserResponse()).Error("Failed to load user"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(new UserResponse { Id = me.Id, Login = me.Login });
    }

    [Fact]
    public async Task AddRole_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.AddRoleToUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).Role.RegisterAsync(app.Id);

        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token)
            .User.AddRoleToUserAsync(
                user.Id,
                role.Id,
                Result.New().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddRole_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.AddRoleToUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddRole_MissingRole_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token)
            .User.AddRoleToUserAsync(
                other.Id,
                Guid.NewGuid(),
                Result.New().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token)
            .User.AddRoleToUserAsync(
                other.Id,
                role.Id,
                Result.New().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteRole_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.DeleteRoleFromUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var owner = await Id(otherToken)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).Role.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.DeleteRoleFromUserAsync(
                owner.Id,
                role.Id,
                Result.New().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteRole_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.DeleteRoleFromUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_MissingRole_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token)
            .User.DeleteRoleFromUserAsync(
                other.Id,
                Guid.NewGuid(),
                Result.New().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token)
            .User.DeleteRoleFromUserAsync(
                other.Id,
                role.Id,
                Result.New().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaim_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).Claim.RegisterAsync(app.Id);
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token).User.AddUserClaimAsync(other.Id, claim.Id, Faker.Random.String2(1));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).Claim.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token).User.AddUserClaimAsync(user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaim_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token).User.AddUserClaimAsync(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaim_MissingClaim_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token).User.AddUserClaimAsync(other.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaim_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).Claim.RegisterAsync(app.Id);
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token).User.AddUserClaimAsync(other.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteClaim_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.DeleteClaimFromUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var owner = await Id(otherToken)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).Claim.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.DeleteClaimFromUserAsync(
                owner.Id,
                claim.Id,
                Result.New().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaim_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .User.DeleteClaimFromUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaim_MissingClaim_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token)
            .User.DeleteClaimFromUserAsync(
                other.Id,
                Guid.NewGuid(),
                Result.New().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaim_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).Claim.RegisterAsync(app.Id);
        var other = await Id().RegisterLogInGetUserAsync();

        // act
        var response = await Id(token)
            .User.DeleteClaimFromUserAsync(
                other.Id,
                claim.Id,
                Result.New().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
