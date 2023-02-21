using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Testing;
using Server.Host.TestClient;
using Xunit;

namespace Server.IntegrationTests.DemoClient.Controllers;

public class DemoControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task IdAuthorization_Unauthorized_ReturnsUnauthorized()
    {
        // act
        var response = await Demo(Guid.NewGuid()).Index.Base();

        // assert
        response.StatusCode.Is(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task IdAuthorization_Authorized_Works()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var app = await Id(token).App.Register();
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Demo(app.Id, token).Index.Base();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        // FIXME: use, when System.Text.Json supports Deserialize with non-default constructor
        // var token = await Demo(app.Id).Get("/base").BearerAuthorization(appTokens.AccessToken).AsAsync<IdToken>();

        // // assert
        // token.IsNotDefault();
        // token.UserId.Is(user.Id);
        // token.LoginId.IsNotDefault();
    }

    [Fact]
    public async Task IdAuthorization_CheckRole_Unauthorized_ReturnsUnauthorized()
    {
        // act
        var response = await Demo(Guid.NewGuid()).Index.IsAdmin();

        // assert
        response.StatusCode.Is(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task IdAuthorization_CheckRole_HasNoAccess_ReturnsForbidden()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var app = await Id(token).App.Register();
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Demo(app.Id, token).Index.IsAdmin();

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task IdAuthorization_CheckRole_HasAccess_Works()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var user = await Id(token).Me.GetMe().GetData();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id, "admin", "Administrator");
        await Id(token).User.AddRoleToUser(role.Id, user.Id);
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Demo(app.Id, token).Index.IsAdmin();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task IdAuthorization_CheckClaim_HasRoleAccess_Works()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var user = await Id(token).Me.GetMe().GetData();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id, "coo", "Chief Operations Officer");
        await Id(token).User.AddRoleToUser(role.Id, user.Id);
        var claim = await Id(token).Claim.Register(app.Id, "paymentsAccess", "Payments Access");
        await Id(token).Role.AddClaimToRole(claim.Id, role.Id, "full");
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Demo(app.Id, token).Index.HasPaymentsAccess();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task IdAuthorization_CheckClaim_HasClaimAccess_Works()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var user = await Id(token).Me.GetMe().GetData();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id, "paymentsAccess", "Payments Access");
        await Id(token).User.AddUserClaim(user.Id, claim.Id, "full");
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Demo(app.Id, token).Index.HasPaymentsAccess();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task IdAuthorization_CheckClaim_HasRoleClaimAccess_Works()
    {
        // arrange
        var login = Faker.Internet.UserName();
        var email = Faker.Internet.Email();
        var password = Faker.Internet.Password();
        var token = await Id().RegisterLogUserIn(login, email, password);
        var user = await Id(token).Me.GetMe().GetData();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id, "coo", "Chief Operations Officer");
        var claim = await Id(token).Claim.Register(app.Id, "paymentsAccess", "Payments Access");
        await Id(token).Role.AddClaimToRole(claim.Id, role.Id, "limited");
        await Id(token).User.AddUserClaim(user.Id, claim.Id, "full");
        token = await Id().LogUserIn(app.Id, login, password);

        // act
        var response = await Demo(app.Id, token).Index.HasPaymentsAccess();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}