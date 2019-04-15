using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;

using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    public class AppUserControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task AddRole_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Put($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Put($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddRole_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await http.Put($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_MissingRole_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Put($"/apps/{app.Id}/users/{other.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_RoleBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var role = await http.Put($"/apps/{app2.Id}/roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<RoleView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Put($"/apps/{app1.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddRole_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<RoleView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Put($"/apps/{app.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteRole_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteRole_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_MissingRole_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{other.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_RoleBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var role = await http.Put($"/apps/{app2.Id}/roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<RoleView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Delete($"/apps/{app1.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteRole_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<RoleView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AddClaim_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await http.Put($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<RoleView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "S" };

            // act
            var response = await http.Post($"/apps/{app.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaim_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_MissingClaim_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/users/{other.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_ClaimBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var claim = await http.Put($"/apps/{app2.Id}/claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "one", Name = "First Claim" }).AsAsync<ClaimView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app1.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await http.Put($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "one", Name = "First Claim" }).AsAsync<ClaimView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<ClaimValueView>();

            // assert
            response.Id.IsEqual(claim.Id);
            response.Key.IsEqual(claim.Key);
            response.Name.IsEqual(claim.Name);
            response.Value.IsEqual(p.Value);
        }

        [Fact]
        public async Task DeleteClaim_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_MissingClaim_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{other.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_ClaimBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var claim = await http.Put($"/apps/{app2.Id}/claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "one", Name = "First Claim" }).AsAsync<ClaimView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Delete($"/apps/{app1.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await http.Put($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "one", Name = "First Claim" }).AsAsync<ClaimView>();
            var other = await RegisterAsync("other", "superpass", "some@email.com");

            // act
            var response = await http.Delete($"/apps/{app.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}