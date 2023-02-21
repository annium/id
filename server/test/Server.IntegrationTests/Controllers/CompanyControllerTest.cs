using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Models.Extensions;
using Annium.Testing;
using Server.Host.TestClient;
using Server.ViewModels.Requests.Companies;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new RegisterCompanyRequest { Name = Faker.Random.String2(2) };

        // act
        var response = await Id(token).Company.RegisterCompany(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_MissingParent_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new RegisterCompanyRequest { ParentId = Guid.NewGuid(), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.RegisterCompany(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NonParentOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var parent = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var request = new RegisterCompanyRequest { ParentId = parent.Id, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.RegisterCompany(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var companyName = Faker.Random.String2(10);

        // act
        var company = await Id(token).Company.Register(name: companyName);

        // assert
        company.Id.IsNotDefault();
        company.Name.Is(companyName);
    }

    [Fact]
    public async Task Find_All_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();

        // act
        var res = await Id(token).Company.FindCompanies(string.Empty);
        var response = res.Data.Data.ToArray();

        // assert
        response.IsShallowEqual(new[] { company });
    }

    [Fact]
    public async Task Find_Query_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();

        // act
        var response = await Id(token).Company.FindCompanies(company.Name).GetData();

        // assert
        response.IsShallowEqual(new[] { company });
    }

    [Fact]
    public async Task ListMy_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();

        // act
        var response = await Id(token).Company.ListMyCompanies().GetData();

        // assert
        response.IsShallowEqual(new[] { company });
    }

    [Fact]
    public async Task Get_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();

        // act
        var response = await Id(token).Company.GetCompany(company.Id).GetData();

        // assert
        response.IsShallowEqual(company);
    }

    [Fact]
    public async Task Get_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Company.GetCompany(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();

        // act
        var response = await Id(token).Company.GetCompany(company.Id).GetData();

        // assert
        response.Id.Is(company.Id);
        response.Name.Is(company.Name);
    }

    [Fact]
    public async Task GetUsers_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Company.GetCompanyUsers(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUsers_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();
        var company = await Id(token).Company.Register();
        await Id(token).CompanyUser.AddUser(company.Id, user.Id);

        // act
        var response = await Id(token).Company.GetCompanyUsers(company.Id).GetData();

        // assert
        response.Has(1);
        response.At(0).Id.Is(user.Id);
        response.At(0).Login.Is(user.Login);
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(2) };

        // act
        var response = await Id(token).Company.UpdateCompany(company.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.UpdateCompany(Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.UpdateCompany(company.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_MissingParent_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();
        var request = new UpdateCompanyRequestBody { ParentId = Guid.NewGuid(), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.UpdateCompany(company.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NonParentOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var parent = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();
        var request = new UpdateCompanyRequestBody { ParentId = parent.Id, Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.UpdateCompany(company.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();
        var request = new UpdateCompanyRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Company.UpdateCompany(company.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SetOwner_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Company.SetCompanyOwner(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).Company.SetCompanyOwner(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SetOwner_MissingSuccessor_NotFound()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();

        // act
        var response = await Id(otherToken).Company.SetCompanyOwner(company.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_Valid_Ok()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();
        var user = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(otherToken).Company.SetCompanyOwner(company.Id, user.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingCompany_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Company.UnregisterCompany(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var company = await Id(otherToken).Company.Register();
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Company.UnregisterCompany(company.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var company = await Id(token).Company.Register();

        // act
        var response = await Id(token).Company.UnregisterCompany(company.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}