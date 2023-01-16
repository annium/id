using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Models.Extensions;
using Annium.Testing;
using Server.TestClient;
using Server.ViewModels.Responses.Users;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class UserControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task FindUsers_Ok()
    {
        // arrange
        var me = await Id().RegisterLogInGetUser();

        // act
        var response = await Id().User.FindUsers(1, me.Login).GetData();

        // assert
        response.IsShallowEqual(new[]
        {
            new UserResponse
            {
                Id = me.Id,
                Login = me.Login,
            }
        });
    }

    [Fact]
    public async Task GetUser_Ok()
    {
        // arrange
        var me = await Id().RegisterLogInGetUser();

        // act
        var response = await Id().User.GetUser(me.Id).GetData();

        // assert
        response.IsShallowEqual(new UserResponse
        {
            Id = me.Id,
            Login = me.Login,
        });
    }

    [Fact]
    public async Task AddRole_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.AddRoleToUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);

        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).User.AddRoleToUser(role.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddRole_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.AddRoleToUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddRole_MissingRole_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.AddRoleToUser(Guid.NewGuid(), other.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.AddRoleToUser(role.Id, other.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteRole_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.DeleteRoleFromUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var owner = await Id(otherToken).Me.GetMe().GetData();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.DeleteRoleFromUser(role.Id, owner.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteRole_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.DeleteRoleFromUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_MissingRole_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.DeleteRoleFromUser(Guid.NewGuid(), other.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.DeleteRoleFromUser(role.Id, other.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaim_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.AddUserClaim(other.Id, claim.Id, "S");

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).User.AddUserClaim(user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaim_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.AddUserClaim(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaim_MissingClaim_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.AddUserClaim(other.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaim_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.AddUserClaim(other.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteClaim_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.DeleteClaimFromUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var owner = await Id(otherToken).Me.GetMe().GetData();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.DeleteClaimFromUser(claim.Id, owner.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaim_MissingUser_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).User.DeleteClaimFromUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaim_MissingClaim_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.DeleteClaimFromUser(Guid.NewGuid(), other.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaim_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);
        var other = await Id().RegisterLogInGetOtherUser();

        // act
        var response = await Id(token).User.DeleteClaimFromUser(claim.Id, other.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}