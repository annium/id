using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Responses.Me;
using Server.ViewModels.Responses.Users;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyUserControllerTest : IntegrationTestBase
{
    public CompanyUserControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task AddUser_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserAsync(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUser_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserAsync(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUser_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserAsync(company.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUser_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);
        var members = await Id(token)
            .Company.GetCompanyUsersAsync(
                company.Id,
                Result.Create(Array.Empty<UserResponse>()).Error("Failed to list company users"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

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
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserRoleAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).CompanyRole.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserRoleAsync(company.Id, user.Id, role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserRole_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRoleAsync(company.Id, Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).CompanyRole.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRoleAsync(company.Id, user.Id, role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserRole_MissingRole_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRoleAsync(company.Id, user.Id, Guid.NewGuid());
        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).CompanyRole.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserRoleAsync(company.Id, user.Id, role.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteUserRole_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .CompanyUser.DeleteCompanyRoleFromCompanyUserAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserRole_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).CompanyRole.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token)
            .CompanyUser.DeleteCompanyRoleFromCompanyUserAsync(
                company.Id,
                user.Id,
                role.Id,
                Result.Create().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserRole_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyRoleFromCompanyUserAsync(
                company.Id,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserRole_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).CompanyRole.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyRoleFromCompanyUserAsync(
                company.Id,
                user.Id,
                role.Id,
                Result.Create().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserRole_MissingRole_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyRoleFromCompanyUserAsync(
                company.Id,
                user.Id,
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserRole_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var role = await Id(otherToken).CompanyRole.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);
        await Id(otherToken).CompanyUser.AddUserRoleAsync(company.Id, user.Id, role.Id);

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyRoleFromCompanyUserAsync(
                company.Id,
                user.Id,
                role.Id,
                Result.Create().Error("Failed to delete role from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddUserClaim_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .CompanyUser.AddUserClaimAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Faker.Random.String2(1));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddUserClaim_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserClaimAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserClaimAsync(company.Id, user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserClaim_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaimAsync(company.Id, Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserClaim_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaimAsync(company.Id, user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AddUserClaim_MissingClaim_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaimAsync(company.Id, user.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserClaim_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken).CompanyUser.AddUserClaimAsync(company.Id, user.Id, claim.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteUserClaim_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token).CompanyUser.AddUserClaimAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserClaim_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyClaimFromCompanyUserAsync(
                company.Id,
                user.Id,
                claim.Id,
                Result.Create().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserClaim_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyClaimFromCompanyUserAsync(
                company.Id,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserClaim_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyClaimFromCompanyUserAsync(
                company.Id,
                user.Id,
                claim.Id,
                Result.Create().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUserClaim_MissingClaim_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyClaimFromCompanyUserAsync(
                company.Id,
                user.Id,
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUserClaim_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);
        await Id(otherToken).CompanyUser.AddUserClaimAsync(company.Id, user.Id, claim.Id);

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteCompanyClaimFromCompanyUserAsync(
                company.Id,
                user.Id,
                claim.Id,
                Result.Create().Error("Failed to delete claim from user"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteUser_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .CompanyUser.DeleteUserFromCompanyAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete user from company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token)
            .CompanyUser.DeleteUserFromCompanyAsync(
                company.Id,
                user.Id,
                Result.Create().Error("Failed to delete user from company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_MissingUser_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteUserFromCompanyAsync(
                company.Id,
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete user from company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_NotMember_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteUserFromCompanyAsync(
                company.Id,
                user.Id,
                Result.Create().Error("Failed to delete user from company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteUser_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        await Id(otherToken).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(otherToken)
            .CompanyUser.DeleteUserFromCompanyAsync(
                company.Id,
                user.Id,
                Result.Create().Error("Failed to delete user from company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
