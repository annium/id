using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Testing;
using Server.TestClient;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyUserControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task AddUser_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.AddUser(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUser_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).CompanyUser.AddUser(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUser_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).CompanyUser.AddUser(company.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUser_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);
        var members = await Id(token).Company.GetCompanyUsers(company.Id).GetData();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        members.Has(1);
        var member = members.At(0);
        member.Id.Is(user.Id);
    }

    [Fact]
    public async Task AddUserRole_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.AddUserRole(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).CompanyUser.AddUserRole(company.Id, user.Id, role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserRole_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRole(company.Id, Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRole(company.Id, user.Id, role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserRole_MissingRole_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRole(company.Id, user.Id, Guid.NewGuid());
        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRole(company.Id, user.Id, role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteUserRole_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.DeleteCompanyRoleFromCompanyUser(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).CompanyUser.DeleteCompanyRoleFromCompanyUser(company.Id, role.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserRole_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyRoleFromCompanyUser(company.Id, Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserRole_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyRoleFromCompanyUser(company.Id, role.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserRole_MissingRole_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyRoleFromCompanyUser(company.Id, Guid.NewGuid(), user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserRole_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var role = await Id(otherToken).CompanyRole.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);
        await Id(otherToken).CompanyUser.AddUserRole(company.Id, user.Id, role.Id);

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyRoleFromCompanyUser(company.Id, role.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddUserClaim_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.AddUserClaim(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Faker.Random.String2(1));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddUserClaim_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.AddUserClaim(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).CompanyUser.AddUserClaim(company.Id, user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserClaim_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaim(company.Id, Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserClaim_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaim(company.Id, user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserClaim_MissingClaim_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaim(company.Id, user.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserClaim_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaim(company.Id, user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteUserClaim_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.AddUserClaim(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyClaimFromCompanyUser(claim.Id, company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserClaim_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyClaimFromCompanyUser(Guid.NewGuid(), company.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserClaim_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyClaimFromCompanyUser(claim.Id, company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserClaim_MissingClaim_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyClaimFromCompanyUser(Guid.NewGuid(), company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserClaim_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).CompanyClaim.Register(app.Id);
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);
        await Id(otherToken).CompanyUser.AddUserClaim(company.Id, user.Id, claim.Id);

        // act
        var response = await Id(otherToken).CompanyUser.DeleteCompanyClaimFromCompanyUser(claim.Id, company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteUser_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).CompanyUser.DeleteUserFromCompany(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).CompanyUser.DeleteUserFromCompany(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteUserFromCompany(company.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).CompanyUser.DeleteUserFromCompany(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        await Id(otherToken).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.DeleteUserFromCompany(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}