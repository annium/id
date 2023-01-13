using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Testing;
using Server.TestClient;
using Server.ViewModels.Requests.CompanyRoles;
using Server.ViewModels.Responses.CompanyClaims;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyRoleControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateCompanyRoleRequest { AppId = Guid.NewGuid(), Key = "one" };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateCompanyRoleRequest { AppId = Guid.NewGuid(), Key = "one", Name = "First Role" };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

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
        var request = new CreateCompanyRoleRequest { AppId = app.Id, Key = "one", Name = "First Role" };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

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
        await Id(token).CompanyRole.Register(app.Id, roleKey, roleName);
        var request = new CreateCompanyRoleRequest { AppId = app.Id, Key = "one", Name = "First Role" };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

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
        var role = await Id(token).CompanyRole.Register(app.Id, roleKey, roleName);

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
        var response = await Id(token).CompanyRole.ListRoles(Guid.NewGuid());

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
        var role = await Id(token).CompanyRole.Register(app.Id, roleKey, roleName);

        // act
        var roles = await Id(token).CompanyRole.ListRoles(app.Id).GetData();

        // assert
        roles.Has(1);
        roles.At(0).Id.IsEqual(role.Id);
        roles.At(0).Claims.IsEmpty();
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRoleRequestBody { Key = "one" };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(Guid.NewGuid(), request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRoleRequestBody { Key = "one", Name = "One Role" };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(Guid.NewGuid(), request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRoleRequestBody { Key = "one", Name = "One Role" };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var otherRole = await Id(token).CompanyRole.RegisterOther(app.Id);
        var request = new UpdateCompanyRoleRequestBody { Key = otherRole.Key, Name = otherRole.Name };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var request = new UpdateCompanyRoleRequestBody { Key = "one", Name = "One Role" };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaimToRole_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(Guid.NewGuid(), Guid.NewGuid(), "S");

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaimToRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(Guid.NewGuid(), role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(claim.Id, role.Id);

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
        var role = await Id(token).CompanyRole.Register(app2.Id);
        var claim = await Id(token).CompanyClaim.Register(app1.Id);

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(claim.Id, role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var claim = await Id(token).CompanyClaim.Register(app.Id);
        var claimValue = "Some";

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(claim.Id, role.Id, claimValue);
        var roles = await Id(token).CompanyRole.ListRoles(app.Id).GetData();

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
        roles.Has(1);
        roles.At(0).Id.IsEqual(role.Id);
        var c = roles.At(0).Claims.At(0);
        c.IsEqual(new CompanyClaimValueResponse { Id = claim.Id, Key = claim.Key, Name = claim.Name, Value = claimValue });
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(Guid.NewGuid(), role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(claim.Id, role.Id);

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
        var role = await Id(token).CompanyRole.Register(app2.Id);
        var claim = await Id(token).CompanyClaim.Register(app1.Id);

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(claim.Id, role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteClaimFromRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var claim = await Id(token).CompanyClaim.Register(app.Id);

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(claim.Id, role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteRole(Guid.NewGuid());

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogOtherUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteRole(role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);

        // act
        var response = await Id(token).CompanyRole.DeleteRole(role.Id);

        // assert
        response.StatusCode.IsEqual(HttpStatusCode.OK);
    }
}