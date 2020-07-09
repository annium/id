using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.TestClient;
using Annium.Id.Api.TestClient.Clients;
using Annium.Id.Api.ViewModels.Requests.Companies;
using Annium.Testing;
using Xunit;

namespace Annium.Id.Api.IntegrationTests.Controllers
{
    public class CompanyControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new RegisterCompanyRequest { Name = "xx" };

            // act
            var response = await Id(token).Company.RegisterCompany(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_MissingParent_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new RegisterCompanyRequest { ParentId = Guid.NewGuid(), Name = "Demo Company" };

            // act
            var response = await Id(token).Company.RegisterCompany(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NonParentOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var parent = await Id(otherToken).Company.RegisterOther();
            var token = await Id().RegisterLogUserIn();
            var request = new RegisterCompanyRequest { ParentId = parent.Id, Name = "Demo Company" };

            // act
            var response = await Id(token).Company.RegisterCompany(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var companyName = "Demo Company";

            // act
            var company = await Id(token).Company.Register(name: companyName);

            // assert
            company.Id.IsNotDefault();
            company.Name.IsEqual(companyName);
        }

        [Fact]
        public async Task GetInfo_MissingCompany_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Company.GetCompanyInfo(Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetInfo_Valid_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var company = await Id(token).Company.Register();

            // act
            var response = await Id(token).Company.GetCompanyInfo(company.Id).GetData();

            // assert
            response.IsEqual(company);
        }

        [Fact]
        public async Task GetUsers_MissingCompany_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Company.GetCompanyUsers(Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
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
            response.At(0).IsEqual(new { user.Id, user.Login });
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var company = await Id(token).Company.Register();
            var request = new UpdateCompanyRequestBody { Name = "xx" };

            // act
            var response = await Id(token).Company.UpdateCompany(company.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingCompany_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateCompanyRequestBody { Name = "Demo Company" };

            // act
            var response = await Id(token).Company.UpdateCompany(Guid.NewGuid(), request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var company = await Id(otherToken).Company.Register();
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateCompanyRequestBody { Name = "Demo Company" };

            // act
            var response = await Id(token).Company.UpdateCompany(company.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingParent_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var company = await Id(token).Company.Register();
            var request = new UpdateCompanyRequestBody { ParentId = Guid.NewGuid(), Name = "Demo Company" };

            // act
            var response = await Id(token).Company.UpdateCompany(company.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NonParentOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var parent = await Id(otherToken).Company.RegisterOther();
            var token = await Id().RegisterLogUserIn();
            var company = await Id(token).Company.Register();
            var request = new UpdateCompanyRequestBody { ParentId = parent.Id, Name = "Demo Company" };

            // act
            var response = await Id(token).Company.UpdateCompany(company.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var company = await Id(token).Company.Register();
            var request = new UpdateCompanyRequestBody { Name = "Some Company" };

            // act
            var response = await Id(token).Company.UpdateCompany(company.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SetOwner_MissingCompany_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Company.SetCompanyOwner(Guid.NewGuid(), Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var company = await Id(otherToken).Company.Register();
            var token = await Id().RegisterLogUserIn();
            var user = await Id(token).Me.GetMe().GetData();

            // act
            var response = await Id(token).Company.SetCompanyOwner(company.Id, user.Id);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var company = await Id(otherToken).Company.Register();

            // act
            var response = await Id(otherToken).Company.SetCompanyOwner(company.Id, Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var company = await Id(otherToken).Company.Register();
            var token = await Id().RegisterLogUserIn();
            var user = await Id(token).Me.GetMe().GetData();

            // act
            var response = await Id(otherToken).Company.SetCompanyOwner(company.Id, user.Id);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_MissingCompany_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Company.UnregisterCompany(Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var company = await Id(otherToken).Company.Register();
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Company.UnregisterCompany(company.Id);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
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
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}