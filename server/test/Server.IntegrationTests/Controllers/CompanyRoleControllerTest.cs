using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.CompanyRoles;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyRoleControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateCompanyRoleRequest { AppId = Guid.NewGuid(), Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateCompanyRoleRequest { AppId = Guid.NewGuid(), Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

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
        var request = new CreateCompanyRoleRequest { AppId = app.Id, Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

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
        await Id(token).CompanyRole.Register(app.Id, roleKey, roleName);
        var request = new CreateCompanyRoleRequest { AppId = app.Id, Key = roleKey, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).CompanyRole.CreateRole(request);

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
        var role = await Id(token).CompanyRole.Register(app.Id, roleKey, roleName);

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
        var response = await Id(token).CompanyRole.ListRoles(Guid.NewGuid());

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
        var role = await Id(token).CompanyRole.Register(app.Id, roleKey, roleName);

        // act
        var roles = await Id(token).CompanyRole.ListRoles(app.Id).GetData();

        // assert
        roles.Has(1);
        roles.At(0).Id.Is(role.Id);
        roles.At(0).Claims.IsEmpty();
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRoleRequestBody { Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRoleRequestBody { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRoleRequestBody { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var otherRole = await Id(token).CompanyRole.Register(app.Id);
        var request = new UpdateCompanyRoleRequestBody { Key = otherRole.Key, Name = otherRole.Name };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var request = new UpdateCompanyRoleRequestBody { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).CompanyRole.UpdateRole(role.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddClaimToRole_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(Guid.NewGuid(), Guid.NewGuid(), Faker.Random.String2(1));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddClaimToRole_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(role.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddClaimToRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(role.Id, claim.Id);

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
        var role = await Id(token).CompanyRole.Register(app2.Id);
        var claim = await Id(token).CompanyClaim.Register(app1.Id);

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(role.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddClaimToRole_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);
        var claim = await Id(token).CompanyClaim.Register(app.Id);
        var claimValue = Faker.Random.String2(5);

        // act
        var response = await Id(token).CompanyRole.AddClaimToRole(role.Id, claim.Id, claimValue);
        var roles = await Id(token).CompanyRole.ListRoles(app.Id).GetData();

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
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var role = await Id(token).CompanyRole.Register(app.Id);

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(role.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteClaimFromRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(role.Id, claim.Id);

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
        var role = await Id(token).CompanyRole.Register(app2.Id);
        var claim = await Id(token).CompanyClaim.Register(app1.Id);

        // act
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(role.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
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
        var response = await Id(token).CompanyRole.DeleteClaimFromRole(role.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingRole_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteRole(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyRole.DeleteRole(role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
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
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}