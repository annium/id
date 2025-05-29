using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Models.Extensions;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Companies;
using Server.ViewModels.Responses.Companies;
using Server.ViewModels.Responses.Me;
using Server.ViewModels.Responses.Users;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyControllerTest : IntegrationTestBase
{
    public CompanyControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new RegisterCompanyRequest { Name = Faker.Random.String2(2) };

        // act
        var response = await Id(token)
            .Company.RegisterCompanyAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to register company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_MissingParent_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new RegisterCompanyRequest { ParentId = Guid.NewGuid(), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.RegisterCompanyAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to register company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NonParentOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var parent = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var request = new RegisterCompanyRequest { ParentId = parent.Id, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.RegisterCompanyAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to register company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var companyName = Faker.Random.String2(10);

        // act
        var company = await Id(token).Company.RegisterAsync(name: companyName);

        // assert
        company.Id.IsNotDefault();
        company.Name.Is(companyName);
    }

    [Fact]
    public async Task Find_All_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();

        // act
        var res = await Id(token)
            .Company.FindCompaniesAsync(
                string.Empty,
                Result.New(Array.Empty<CompanyResponse>()).Error("Failed to find companies"),
                TestContext.Current.CancellationToken
            );
        var response = res.Data.Data.ToArray();

        // assert
        response.IsShallowEqual(new[] { company });
    }

    [Fact]
    public async Task Find_Query_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();

        // act
        var response = await Id(token)
            .Company.FindCompaniesAsync(
                company.Name,
                Result.New(Array.Empty<CompanyResponse>()).Error("Failed to find companies"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(new[] { company });
    }

    [Fact]
    public async Task ListMy_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();

        // act
        var response = await Id(token)
            .Company.ListMyCompaniesAsync(
                Result.New(Array.Empty<CompanyResponse>()).Error("Failed to list my companies"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(new[] { company });
    }

    [Fact]
    public async Task Get_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();

        // act
        var response = await Id(token)
            .Company.GetCompanyAsync(
                company.Id,
                Result.New(new CompanyResponse()).Error("Failed to load company info"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(company);
    }

    [Fact]
    public async Task Get_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Company.GetCompanyAsync(
                Guid.NewGuid(),
                Result.New(new CompanyResponse()).Error("Failed to load company info"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();

        // act
        var response = await Id(token)
            .Company.GetCompanyAsync(
                company.Id,
                Result.New(new CompanyResponse()).Error("Failed to load company info"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.Id.Is(company.Id);
        response.Name.Is(company.Name);
    }

    [Fact]
    public async Task GetUsers_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Company.GetCompanyUsersAsync(
                Guid.NewGuid(),
                Result.New(Array.Empty<UserResponse>()).Error("Failed to list company users"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUsers_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();
        var company = await Id(token).Company.RegisterAsync();
        await Id(token).CompanyUser.AddUserAsync(company.Id, user.Id);

        // act
        var response = await Id(token)
            .Company.GetCompanyUsersAsync(
                company.Id,
                Result.New(Array.Empty<UserResponse>()).Error("Failed to list company users"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.Has(1);
        response.At(0).Id.Is(user.Id);
        response.At(0).Login.Is(user.Login);
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(2) };

        // act
        var response = await Id(token)
            .Company.UpdateCompanyAsync(
                company.Id,
                request,
                Result.New().Error("Failed to update company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.UpdateCompanyAsync(
                Guid.NewGuid(),
                request,
                Result.New().Error("Failed to update company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.UpdateCompanyAsync(
                company.Id,
                request,
                Result.New().Error("Failed to update company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_MissingParent_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();
        var request = new UpdateCompanyRequestBody { ParentId = Guid.NewGuid(), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.UpdateCompanyAsync(
                company.Id,
                request,
                Result.New().Error("Failed to update company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NonParentOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var parent = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var company = await Id(token).Company.RegisterAsync();
        var request = new UpdateCompanyRequestBody { ParentId = parent.Id, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.UpdateCompanyAsync(
                company.Id,
                request,
                Result.New().Error("Failed to update company"),
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
        var company = await Id(token).Company.RegisterAsync();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .Company.UpdateCompanyAsync(
                company.Id,
                request,
                Result.New().Error("Failed to update company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SetOwner_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Company.SetCompanyOwnerAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.New().Error("Failed to set company owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token)
            .Company.SetCompanyOwnerAsync(
                company.Id,
                user.Id,
                Result.New().Error("Failed to set company owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SetOwner_MissingSuccessor_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();

        // act
        var response = await Id(otherToken)
            .Company.SetCompanyOwnerAsync(
                company.Id,
                Guid.NewGuid(),
                Result.New().Error("Failed to set company owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var user = await Id(token)
            .Me.GetMeAsync(
                Result.New(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(otherToken)
            .Company.SetCompanyOwnerAsync(
                company.Id,
                user.Id,
                Result.New().Error("Failed to set company owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Company.UnregisterCompanyAsync(
                Guid.NewGuid(),
                Result.New().Error("Failed to unregister company"),
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
        var company = await Id(otherToken).Company.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .Company.UnregisterCompanyAsync(
                company.Id,
                Result.New().Error("Failed to unregister company"),
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
        var company = await Id(token).Company.RegisterAsync();

        // act
        var response = await Id(token)
            .Company.UnregisterCompanyAsync(
                company.Id,
                Result.New().Error("Failed to unregister company"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
