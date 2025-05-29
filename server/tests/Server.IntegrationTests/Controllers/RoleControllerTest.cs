using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Roles;
using Server.ViewModels.Responses.Roles;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class RoleControllerTest : IntegrationTestBase
{
    public RoleControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateRoleRequest { Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .Role.CreateRoleAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateRoleRequest
        {
            AppId = Guid.NewGuid(),
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .Role.CreateRoleAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateRoleRequest
        {
            AppId = app.Id,
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .Role.CreateRoleAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_NonUniqueKey_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var roleKey = Faker.Random.String2(5);
        var roleName = Faker.Random.String2(10);
        await Id(token).Role.RegisterAsync(app.Id, roleKey, roleName);
        var request = new CreateRoleRequest
        {
            AppId = app.Id,
            Key = roleKey,
            Name = roleName,
        };

        // act
        var response = await Id(token)
            .Role.CreateRoleAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var roleKey = Faker.Random.String2(5);
        var roleName = Faker.Random.String2(10);

        // act
        var role = await Id(token).Role.RegisterAsync(app.Id, roleKey, roleName);

        // assert
        role.Id.IsNotDefault();
        role.AppId.Is(app.Id);
        role.Key.Is(roleKey);
        role.Name.Is(roleName);
    }

    [Fact]
    public async Task List_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Role.ListRolesAsync(
                Guid.NewGuid(),
                Result.New(Array.Empty<RoleResponse>()).Error("Failed to list roles"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task List_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var roleKey = Faker.Random.String2(5);
        var roleName = Faker.Random.String2(10);
        var role = await Id(token).Role.RegisterAsync(app.Id, roleKey, roleName);

        // act
        var roles = await Id(token)
            .Role.ListRolesAsync(
                app.Id,
                Result.New(Array.Empty<RoleResponse>()).Error("Failed to list roles"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        roles.Has(1);
        roles.At(0).Id.Is(role.Id);
        roles.At(0).AppId.Is(role.AppId);
        roles.At(0).Key.Is(role.Key);
        roles.At(0).Name.Is(role.Name);
        roles.At(0).Claims.IsEmpty();
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .Role.UpdateRoleAsync(
                role.Id,
                request,
                Result.New().Error("Failed to update role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Role.UpdateRoleAsync(
                Guid.NewGuid(),
                request,
                Result.New().Error("Failed to update role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var otherRole = await Id(token).Role.RegisterAsync(app.Id);
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var request = new UpdateRoleRequest { Key = otherRole.Key, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Role.UpdateRoleAsync(
                role.Id,
                request,
                Result.New().Error("Failed to update role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).Role.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Role.UpdateRoleAsync(
                role.Id,
                request,
                Result.New().Error("Failed to update role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Role.UpdateRoleAsync(
                role.Id,
                request,
                Result.New().Error("Failed to update role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaimToRole_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var claim = await Id(token).Claim.RegisterAsync(app.Id);
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(1) };

        // act
        var response = await Id(token)
            .Role.AddClaimToRoleAsync(
                role.Id,
                claim.Id,
                request,
                Result.New().Error("Failed to add claim to role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaimToRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .Role.AddClaimToRoleAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                request,
                Result.New().Error("Failed to add claim to role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .Role.AddClaimToRoleAsync(
                role.Id,
                Guid.NewGuid(),
                request,
                Result.New().Error("Failed to add claim to role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).Role.RegisterAsync(app.Id);
        var claim = await Id(otherToken).Claim.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .Role.AddClaimToRoleAsync(
                role.Id,
                claim.Id,
                request,
                Result.New().Error("Failed to add claim to role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_ClaimBelongsOtherApp_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app1 = await Id(token).App.RegisterAsync();
        var app2 = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app2.Id);
        var claim = await Id(token).Claim.RegisterAsync(app1.Id);
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .Role.AddClaimToRoleAsync(
                role.Id,
                claim.Id,
                request,
                Result.New().Error("Failed to add claim to role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var claim = await Id(token).Claim.RegisterAsync(app.Id);
        var claimValue = Faker.Random.String2(5);

        // act
        var response = await Id(token).Role.AddClaimToRoleAsync(role.Id, claim.Id, claimValue);
        var roles = await Id(token)
            .Role.ListRolesAsync(
                app.Id,
                Result.New(Array.Empty<RoleResponse>()).Error("Failed to list roles"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        roles.Has(1);
        roles.At(0).Id.Is(role.Id);
        var c = roles.At(0).Claims.At(0);
        c.RoleId.Is(role.Id);
        c.ClaimId.Is(claim.Id);
        c.Claim.Id.Is(claim.Id);
        c.Claim.AppId.Is(claim.AppId);
        c.Claim.Key.Is(claim.Key);
        c.Claim.Name.Is(claim.Name);
        c.Value.Is(claimValue);
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Role.DeleteClaimFromRoleAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to delete claim from role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);

        // act
        var response = await Id(token)
            .Role.DeleteClaimFromRoleAsync(
                role.Id,
                Guid.NewGuid(),
                Result.New().Error("Failed to delete claim from role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).Role.RegisterAsync(app.Id);
        var claim = await Id(otherToken).Claim.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Role.DeleteClaimFromRoleAsync(
                role.Id,
                claim.Id,
                Result.New().Error("Failed to delete claim from role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaimFromRole_ClaimBelongsOtherApp_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app1 = await Id(token).App.RegisterAsync();
        var app2 = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app2.Id);
        var claim = await Id(token).Claim.RegisterAsync(app1.Id);

        // act
        var response = await Id(token)
            .Role.DeleteClaimFromRoleAsync(
                role.Id,
                claim.Id,
                Result.New().Error("Failed to delete claim from role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaimFromRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);
        var claim = await Id(token).Claim.RegisterAsync(app.Id);

        // act
        var response = await Id(token)
            .Role.DeleteClaimFromRoleAsync(
                role.Id,
                claim.Id,
                Result.New().Error("Failed to delete claim from role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Role.DeleteRoleAsync(
                Guid.NewGuid(),
                Result.New().Error("Failed to delete role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var role = await Id(otherToken).Role.RegisterAsync(app.Id);

        // act
        var response = await Id(token)
            .Role.DeleteRoleAsync(
                role.Id,
                Result.New().Error("Failed to delete role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var role = await Id(token).Role.RegisterAsync(app.Id);

        // act
        var response = await Id(token)
            .Role.DeleteRoleAsync(
                role.Id,
                Result.New().Error("Failed to delete role"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
