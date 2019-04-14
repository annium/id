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
    public class OrganizationControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new OrganizationPayload() { Key = "de", Name = "Demo Organization" };

            // act
            var response = await http.Put("/organizations").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_MissingParent_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new OrganizationPayload() { ParentId = Guid.NewGuid(), Key = "demo", Name = "Demo Organization" };
            await http.Put("/organizations").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await http.Put("/organizations").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new OrganizationPayload() { Key = "demo", Name = "Demo Organization" };
            await http.Put("/organizations").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await http.Put("/organizations").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new OrganizationPayload() { Key = "demo", Name = "Demo Organization" };

            // act
            var organization = await http.Put("/organizations").BearerAuthorization(tokens.AccessToken).JsonContent(payload).AsAsync<OrganizationView>();

            // assert
            organization.Id.IsNotDefault();
            organization.Key.IsEqual(payload.Key);
            organization.Name.IsEqual(payload.Name);
        }

        [Fact]
        public async Task GetInfo_MissingOrganization_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Get($"/organizations/{Guid.NewGuid()}").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetInfo_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var organization = await CreateOrganizationAsync(tokens.AccessToken);

            // act
            var response = await http.Get($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).AsAsync<OrganizationView>();

            // assert
            response.Id.IsEqual(organization.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var organization = await CreateOrganizationAsync(tokens.AccessToken);
            var u = new OrganizationPayload { Key = "demo" };

            // act
            var response = await http.Post($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingOrganization_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var u = new OrganizationPayload { Key = "demo", Name = "Demo Organization" };

            // act
            var response = await http.Post($"/organizations/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var organization = await CreateOrganizationAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var u = new OrganizationPayload { Key = "demo", Name = "Demo Organization" };

            // act
            var response = await http.Post($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingParent_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var organization = await CreateOrganizationAsync(tokens.AccessToken);
            var u = new OrganizationPayload { ParentId = Guid.NewGuid(), Key = "demo", Name = "Demo Organization" };

            // act
            var response = await http.Post($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_KeyIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var organization1 = await CreateOrganizationAsync(tokens.AccessToken);
            var organization2 = await CreateOrganizationAsync(tokens.AccessToken, "medo");
            var u = new OrganizationPayload { Key = "demo", Name = "Some" };

            // act
            var response = await http.Post($"/organizations/{organization2.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var organization = await CreateOrganizationAsync(tokens.AccessToken);
            var u = new OrganizationPayload { Key = "medo", Name = "Medo Organization" };

            // act
            var response = await http.Post($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<OrganizationView>();

            // assert
            response.Id.IsEqual(organization.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
        }

        [Fact]
        public async Task SetOwner_MissingOrganization_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/organizations/{Guid.NewGuid()}/owner/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var organization = await CreateOrganizationAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/organizations/{organization.Id}/owner/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var organization = await CreateOrganizationAsync(ownerTokens.AccessToken);

            // act
            var response = await http.Post($"/organizations/{organization.Id}/owner/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var organization = await CreateOrganizationAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            organization = await http.Post($"/organizations/{organization.Id}/owner/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).AsAsync<OrganizationView>();

            // assert
            organization.OwnerId.IsEqual(user.Id);
        }

        [Fact]
        public async Task Delete_MissingOrganization_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/organizations/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var organization = await CreateOrganizationAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var organization = await CreateOrganizationAsync(tokens.AccessToken);

            // act
            var response = await http.Delete($"/organizations/{organization.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}