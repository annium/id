using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.TestClient;
using Annium.Id.Api.ViewModels.Requests.Roles;
using Annium.Id.Api.ViewModels.Responses.Claims;
using Annium.Id.Api.ViewModels.Responses.Roles;
using Annium.Testing;
using Xunit;

namespace Annium.Id.Api.IntegrationTests.Controllers;

public class RoleControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateRoleRequest { Key = "one" };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateRoleRequest { AppId = Guid.NewGuid(), Key = "one", Name = "First Role" };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var request = new CreateRoleRequest { AppId = app.Id, Key = "one", Name = "First Role" };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_NonUniqueKey_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var roleKey = "one";
        var roleName = "First Role";
        await Id(token).Role.Register(app.Id, roleKey, roleName);
        var request = new CreateRoleRequest { AppId = app.Id, Key = roleKey, Name = roleName };

        // act
        var response = await Id(token).Role.CreateRole(request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var roleKey = "one";
        var roleName = "First Role";

        // act
        var role = await Id(token).Role.Register(app.Id, roleKey, roleName);

        // assert
        role.Id.IsNotDefault();
        role.IsEqual(new { AppId = app.Id, Key = roleKey, Name = roleName });
    }

    [Fact]
    public async Task List_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.ListRoles(Guid.NewGuid());

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task List_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var roleKey = "one";
        var roleName = "First Role";
        var role = await Id(token).Role.Register(app.Id, roleKey, roleName);

        // act
        var roles = await Id(token).Role.ListRoles(app.Id).GetData();

        // assert
        roles.IsEqual(new[]
        {
            new RoleResponse
            {
                Id = role.Id,
                AppId = app.Id,
                Key = roleKey,
                Name = roleName,
                Claims = Array.Empty<ClaimValueResponse>()
            }
        });
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var request = new UpdateRoleRequest { Key = "one" };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateRoleRequest { Key = "one", Name = "One Role" };

        // act
        var response = await Id(token).Role.UpdateRole(Guid.NewGuid(), request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var otherRole = await Id(token).Role.RegisterOther(app.Id);
        var role = await Id(token).Role.Register(app.Id);
        var request = new UpdateRoleRequest { Key = otherRole.Key, Name = "FirstRole" };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateRoleRequest { Key = "one", Name = "One Role" };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var request = new UpdateRoleRequest { Key = "one", Name = "One Role" };

        // act
        var response = await Id(token).Role.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaimToRole_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var claim = await Id(token).Claim.Register(app.Id);
        var request = new AddClaimToRoleRequest { Value = "S" };

        // act
        var response = await Id(token).Role.AddClaimToRole(claim.Id, role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaimToRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new AddClaimToRoleRequest { Value = "Some" };

        // act
        var response = await Id(token).Role.AddClaimToRole(Guid.NewGuid(), Guid.NewGuid(), request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var request = new AddClaimToRoleRequest { Value = "Some" };

        // act
        var response = await Id(token).Role.AddClaimToRole(Guid.NewGuid(), role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new AddClaimToRoleRequest { Value = "Some" };

        // act
        var response = await Id(token).Role.AddClaimToRole(claim.Id, role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_ClaimBelongsOtherApp_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app1 = await Id(token).App.Register();
        var app2 = await Id(token).App.RegisterOther();
        var role = await Id(token).Role.Register(app2.Id);
        var claim = await Id(token).Claim.Register(app1.Id);
        var request = new AddClaimToRoleRequest { Value = "Some" };

        // act
        var response = await Id(token).Role.AddClaimToRole(claim.Id, role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);
        var claim = await Id(token).Claim.Register(app.Id);
        var claimValue = "Some";

        // act
        var response = await Id(token).Role.AddClaimToRole(claim.Id, role.Id, claimValue);
        var roles = await Id(token).Role.ListRoles(app.Id).GetData();

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
        roles.Has(1);
        roles.At(0).Id.IsEqual(role.Id);
        var c = roles.At(0).Claims.At(0);
        c.IsEqual(new ClaimValueResponse { Id = claim.Id, Key = claim.Key, Name = claim.Name, Value = claimValue });
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).Role.Register(app.Id);

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(Guid.NewGuid(), role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).Role.Register(app.Id);
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(claim.Id, role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaimFromRole_ClaimBelongsOtherApp_Forbidden()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app1 = await Id(token).App.Register();
        var app2 = await Id(token).App.RegisterOther();
        var role = await Id(token).Role.Register(app2.Id);
        var claim = await Id(token).Claim.Register(app1.Id);

        // act
        var response = await Id(token).Role.DeleteClaimFromRole(claim.Id, role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
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
        var response = await Id(token).Role.DeleteClaimFromRole(claim.Id, role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Role.DeleteRole(Guid.NewGuid());

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var role = await Id(otherToken).Role.Register(app.Id);

        // act
        var response = await Id(token).Role.DeleteRole(role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
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
        response.StatusCode.IsEqual(HttpStatusCode.OK);
    }
}