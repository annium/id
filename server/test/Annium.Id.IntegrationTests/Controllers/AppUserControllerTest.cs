using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class AppUserControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task AddRole_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Put($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginUserAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Put($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddRole_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await id.Put($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_MissingRole_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Put($"/apps/{app.Id}/users/{other.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_RoleBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var role = await CreateRoleAsync(tokens.AccessToken, app2.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Put($"/apps/{app1.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddRole_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Put($"/apps/{app.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteRole_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginUserAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteRole_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_MissingRole_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{other.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_RoleBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var role = await CreateRoleAsync(tokens.AccessToken, app2.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/apps/{app1.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteRole_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AddClaim_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "S" };

            // act
            var response = await id.Post($"/apps/{app.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaim_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginUserAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginUserAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_MissingClaim_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app.Id}/users/{other.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_ClaimBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var claim = await CreateClaimAsync(tokens.AccessToken, app2.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app1.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");
            var claimValue = "Some";

            // act
            var response = await AddClaimToUserAsync(tokens.AccessToken, app.Id, other.Id, claim.Id, claimValue);

            // assert
            response.Id.IsEqual(claim.Id);
            response.Key.IsEqual(claim.Key);
            response.Name.IsEqual(claim.Name);
            response.Value.IsEqual(claimValue);
        }

        [Fact]
        public async Task DeleteClaim_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginUserAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginUserAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_MissingUser_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_MissingClaim_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{other.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_ClaimBelongsToOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other");
            var claim = await CreateClaimAsync(tokens.AccessToken, app2.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/apps/{app1.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginUserAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var other = await RegisterUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/apps/{app.Id}/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}