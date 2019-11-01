using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.ViewModels.Companies.Requests;
using Annium.Id.ViewModels.Companies.Responses;
using Annium.Id.ViewModels.Users.Responses;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class CompanyControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new RegisterCompanyRequest() { Key = "de", Name = "Demo Company" };

            // act
            var response = await id.Post("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_MissingParent_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new RegisterCompanyRequest() { ParentId = Guid.NewGuid(), Key = "demo", Name = "Demo Company" };

            // act
            var response = await id.Post("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NonUniqueKey_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new RegisterCompanyRequest() { Key = "demo", Name = "Demo Company" };
            await id.Post("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await id.Post("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_NonParentOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "ownerpass", "owner@owner.com");
            var parent = await CreateCompanyAsync(ownerTokens.AccessToken, "parent", "Parent");
            var tokens = await RegisterLogUserInAsync();
            var payload = new RegisterCompanyRequest() { ParentId = parent.Id, Key = "demo", Name = "Demo Company" };

            // act
            var response = await id.Post("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var companyKey = "demo";
            var companyName = "Demo Company";

            // act
            var company = await CreateCompanyAsync(tokens.AccessToken, companyKey, companyName);

            // assert
            company.Id.IsNotDefault();
            company.Key.IsEqual(companyKey);
            company.Name.IsEqual(companyName);
        }

        [Fact]
        public async Task GetInfo_MissingCompany_NotFound()
        {
            // act
            var response = await id.Get($"/companies/{Guid.NewGuid()}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetInfo_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);

            // act
            var response = (await id.Get($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).AsResultAsync<CompanyResponse>()).Data;

            // assert
            response.Id.IsEqual(company.Id);
            response.Key.IsEqual(company.Key);
            response.Name.IsEqual(company.Name);
        }

        [Fact]
        public async Task GetUsers_MissingCompany_NotFound()
        {
            // act
            var response = await id.Get($"/companies/{Guid.NewGuid()}/users").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetUsers_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);
            var company = await CreateCompanyAsync(tokens.AccessToken);
            await AddUserToCompanyAsync(tokens.AccessToken, company.Id, user.Id);

            // act
            var response = (await id.Get($"/companies/{company.Id}/users").BearerAuthorization(tokens.AccessToken).AsResultAsync<UserResponse[]>()).Data;

            // assert
            response.Has(1);
            response.At(0).Id.IsEqual(user.Id);
            response.At(0).Login.IsEqual(user.Login);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var u = new UpdateCompanyRequest { Key = "demo" };

            // act
            var response = await id.Put($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingCompany_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyRequest { Key = "demo", Name = "Demo Company" };

            // act
            var response = await id.Put($"/companies/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyRequest { Key = "demo", Name = "Demo Company" };

            // act
            var response = await id.Put($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingParent_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var u = new UpdateCompanyRequest { ParentId = Guid.NewGuid(), Key = "demo", Name = "Demo Company" };

            // act
            var response = await id.Put($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NonParentOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "ownerpass", "owner@owner.com");
            var parent = await CreateCompanyAsync(ownerTokens.AccessToken, "parent", "Parent");
            var tokens = await RegisterLogUserInAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var payload = new UpdateCompanyRequest() { ParentId = parent.Id, Key = "demo", Name = "Demo Company" };

            // act
            var response = await id.Put($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_KeyIsNotUnique_Conflict()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            await CreateCompanyAsync(tokens.AccessToken);
            var company2 = await CreateCompanyAsync(tokens.AccessToken, "medo");
            var u = new UpdateCompanyRequest { Key = "demo", Name = "Some" };

            // act
            var response = await id.Put($"/companies/{company2.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var u = new UpdateCompanyRequest { Key = "medo", Name = "Medo Company" };

            // act
            var response = await id.Put($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SetOwner_MissingCompany_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Put($"/companies/{Guid.NewGuid()}/owner/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            var response = await id.Put($"/companies/{company.Id}/owner/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);

            // act
            var response = await id.Put($"/companies/{company.Id}/owner/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            var response = await id.Put($"/companies/{company.Id}/owner/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_MissingCompany_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);

            // act
            var response = await id.Delete($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}