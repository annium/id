using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    [Skip]
    public class CompanyControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new CompanyPayload() { Key = "de", Name = "Demo Company" };

            // act
            var response = await http.Put("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_MissingParent_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new CompanyPayload() { ParentId = Guid.NewGuid(), Key = "demo", Name = "Demo Company" };
            await http.Put("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await http.Put("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new CompanyPayload() { Key = "demo", Name = "Demo Company" };
            await http.Put("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await http.Put("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new CompanyPayload() { Key = "demo", Name = "Demo Company" };

            // act
            var company = await http.Put("/companies").BearerAuthorization(tokens.AccessToken).JsonContent(payload).AsAsync<CompanyView>();

            // assert
            company.Id.IsNotDefault();
            company.Key.IsEqual(payload.Key);
            company.Name.IsEqual(payload.Name);
        }

        [Fact]
        public async Task GetInfo_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Get($"/companies/{Guid.NewGuid()}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetInfo_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);

            // act
            var response = await http.Get($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).AsAsync<CompanyView>();

            // assert
            response.Id.IsEqual(company.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var u = new CompanyPayload { Key = "demo" };

            // act
            var response = await http.Post($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingCompany_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var u = new CompanyPayload { Key = "demo", Name = "Demo Company" };

            // act
            var response = await http.Post($"/companies/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var u = new CompanyPayload { Key = "demo", Name = "Demo Company" };

            // act
            var response = await http.Post($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingParent_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var u = new CompanyPayload { ParentId = Guid.NewGuid(), Key = "demo", Name = "Demo Company" };

            // act
            var response = await http.Post($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_KeyIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var company1 = await CreateCompanyAsync(tokens.AccessToken);
            var company2 = await CreateCompanyAsync(tokens.AccessToken, "medo");
            var u = new CompanyPayload { Key = "demo", Name = "Some" };

            // act
            var response = await http.Post($"/companies/{company2.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);
            var u = new CompanyPayload { Key = "medo", Name = "Medo Company" };

            // act
            var response = await http.Post($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<CompanyView>();

            // assert
            response.Id.IsEqual(company.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
        }

        [Fact]
        public async Task SetOwner_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/companies/{Guid.NewGuid()}/owner/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/companies/{company.Id}/owner/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);

            // act
            var response = await http.Post($"/companies/{company.Id}/owner/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            company = await http.Post($"/companies/{company.Id}/owner/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).AsAsync<CompanyView>();

            // assert
            company.OwnerId.IsEqual(user.Id);
        }

        [Fact]
        public async Task Delete_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/companies/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var company = await CreateCompanyAsync(tokens.AccessToken);

            // act
            var response = await http.Delete($"/companies/{company.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}