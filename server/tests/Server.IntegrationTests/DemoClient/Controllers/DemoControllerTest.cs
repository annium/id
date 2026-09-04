using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Responses.Me;
using Xunit;
using IdTokenResponse = Server.DemoHost.ViewModels.IdTokenResponse;

namespace Server.IntegrationTests.DemoClient.Controllers;

public class DemoControllerTest : IntegrationTestBase
{
    public DemoControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task IdAuthorization_Unauthorized_ReturnsUnauthorized()
    {
        // act
        var demo = await DemoAsync(Guid.NewGuid());
        var response = await demo.Index.BaseAsync(new IdTokenResponse(), TestContext.Current.CancellationToken);

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var app = await Id(token).App.RegisterAsync();
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var demo = await DemoAsync(app.Id, token);
        var response = await demo.Index.BaseAsync(new IdTokenResponse(), TestContext.Current.CancellationToken);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        // FIXME: use, when System.Text.Json supports Deserialize with non-default constructor
        // var token = await (await DemoAsync(app.Id)).Get("/base").BearerAuthorization(appTokens.AccessToken).AsAsync<IdToken>();

        // // assert
        // token.IsNotDefault();
        // token.UserId.Is(user.Id);
        // token.LoginId.IsNotDefault();
    }

    [Fact]
    public async Task IdAuthorization_CheckRole_Unauthorized_ReturnsUnauthorized()
    {
        // act
        var demo = await DemoAsync(Guid.NewGuid());
        var response = await demo.Index.IsAdminAsync(new IdTokenResponse(), TestContext.Current.CancellationToken);

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var app = await Id(token).App.RegisterAsync();
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var demo = await DemoAsync(app.Id, token);
        var response = await demo.Index.IsAdminAsync(new IdTokenResponse(), TestContext.Current.CancellationToken);

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id, "admin", "Administrator");
        await Id(token)
            .User.AddRoleToUserAsync(
                user.Id,
                role.Id,
                Result.Create().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var demo = await DemoAsync(app.Id, token);
        var response = await demo.Index.IsAdminAsync(new IdTokenResponse(), TestContext.Current.CancellationToken);

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id, "coo", "Chief Operations Officer");
        await Id(token)
            .User.AddRoleToUserAsync(
                user.Id,
                role.Id,
                Result.Create().Error("Failed to add role to user"),
                TestContext.Current.CancellationToken
            );
        var claim = await Id(token).Claim.RegisterAsync(app.Id, "paymentsAccess", "Payments Access");
        await Id(token).Role.AddClaimToRoleAsync(role.Id, claim.Id, "full");
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var demo = await DemoAsync(app.Id, token);
        var response = await demo.Index.HasPaymentsAccessAsync(
            new IdTokenResponse(),
            TestContext.Current.CancellationToken
        );

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).Claim.RegisterAsync(app.Id, "paymentsAccess", "Payments Access");
        await Id(token).User.AddUserClaimAsync(user.Id, claim.Id, "full");
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var demo = await DemoAsync(app.Id, token);
        var response = await demo.Index.HasPaymentsAccessAsync(
            new IdTokenResponse(),
            TestContext.Current.CancellationToken
        );

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
        var token = await Id().RegisterLogUserInAsync(login, email, password);
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id, "coo", "Chief Operations Officer");
        var claim = await Id(token).Claim.RegisterAsync(app.Id, "paymentsAccess", "Payments Access");
        await Id(token).Role.AddClaimToRoleAsync(role.Id, claim.Id, "limited");
        await Id(token).User.AddUserClaimAsync(user.Id, claim.Id, "full");
        token = await Id().LogUserInAsync(app.Id, login, password);

        // act
        var demo = await DemoAsync(app.Id, token);
        var response = await demo.Index.HasPaymentsAccessAsync(
            new IdTokenResponse(),
            TestContext.Current.CancellationToken
        );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
