using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Roles;
using Xunit;
using Xunit.Abstractions;

namespace Server.IntegrationTests.Controllers;

public class RoleControllerTest : IntegrationTestBase
{
    public RoleControllerTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateRoleRequest { Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateRoleRequest { AppId = Guid.NewGuid(), Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var request = new CreateRoleRequest { AppId = app.Id, Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_NonUniqueKey_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var roleKey = Faker.Random.String2(5);
        var roleName = Faker.Random.String2(10);
        await Id(token).Role.Register(app.Id, roleKey, roleName);
        var request = new CreateRoleRequest { AppId = app.Id, Key = roleKey, Name = roleName };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var roleKey = Faker.Random.String2(5);
        var roleName = Faker.Random.String2(10);

        // act
        var role = await Id(token).Role.Register(app.Id, roleKey, roleName);

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
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.ListRoles(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task List_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var roleKey = Faker.Random.String2(5);
        var roleName = Faker.Random.String2(10);
        var role = await Id(token).Role.Register(app.Id, roleKey, roleName);

        // act
        var roles = await Id(token).Role.ListRoles(app.Id).GetData();

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
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Role.UpdateRole(Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var otherRole = await Id(token).Role.Register(app.Id);
        var role = await Id(token).Role.Register(app.Id);
        var request = new UpdateRoleRequest { Key = otherRole.Key, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var request = new UpdateRoleRequest { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaimToRole_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var claim = await Id(token).Claim.Register(app.Id);
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(1) };

        // act
        var response = await Id(token).Role.AddClaimToRole(role.Id, claim.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaimToRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token).Role.AddClaimToRole(Guid.NewGuid(), Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token).Role.AddClaimToRole(role.Id, Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token).Role.AddClaimToRole(role.Id, claim.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_ClaimBelongsOtherApp_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app1 = await Id(token).App.Register();
        var app2 = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app2.Id);
        var claim = await Id(token).Claim.Register(app1.Id);
        var request = new AddClaimToRoleRequest { Value = Faker.Random.String2(5) };

        // act
        var response = await Id(token).Role.AddClaimToRole(role.Id, claim.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var claim = await Id(token).Claim.Register(app.Id);
        var claimValue = Faker.Random.String2(5);

        // act
        var response = await Id(token).Role.AddClaimToRole(role.Id, claim.Id, claimValue);
        var roles = await Id(token).Role.ListRoles(app.Id).GetData();

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
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(role.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(role.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaimFromRole_ClaimBelongsOtherApp_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app1 = await Id(token).App.Register();
        var app2 = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app2.Id);
        var claim = await Id(token).Claim.Register(app1.Id);

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(role.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaimFromRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var claim = await Id(token).Claim.Register(app.Id);

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(role.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.DeleteRole(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var role = await Id(otherToken).Role.Register(app.Id);

        // act
        var response = await Id(token).Role.DeleteRole(role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);

        // act
        var response = await Id(token).Role.DeleteRole(role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}