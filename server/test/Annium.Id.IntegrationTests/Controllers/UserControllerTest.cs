using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class UserControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task AddRole_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Post($"/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateRoleAsync(ownerTokens.AccessToken, app.Id);

            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            var response = await id.Post($"/users/{user.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddRole_MissingUser_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Post($"/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_MissingRole_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Post($"/users/{other.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddRole_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Post($"/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteRole_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var owner = await GetUserAsync(ownerTokens.AccessToken);
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateRoleAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/users/{owner.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteRole_MissingUser_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_MissingRole_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/users/{other.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteRole_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/users/{other.Id}/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddClaim_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");
            var p = new AddClaimToUserRequest { Value = "S" };

            // act
            var response = await id.Post($"/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaim_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new AddClaimToUserRequest { Value = "Some" };

            // act
            var response = await id.Post($"/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var claim = await CreateClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);
            var p = new AddClaimToUserRequest { Value = "Some" };

            // act
            var response = await id.Post($"/users/{user.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_MissingUser_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new AddClaimToUserRequest { Value = "Some" };

            // act
            var response = await id.Post($"/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_MissingClaim_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");
            var p = new AddClaimToUserRequest { Value = "Some" };

            // act
            var response = await id.Post($"/users/{other.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");
            var p = new AddClaimToUserRequest { Value = "Some" };

            // act
            var response = await id.Post($"/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteClaim_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var owner = await GetUserAsync(ownerTokens.AccessToken);
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var claim = await CreateClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/users/{owner.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_MissingUser_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_MissingClaim_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/users/{other.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var other = await RegisterLoginGetUserAsync("other", "superpass", "some@email.com");

            // act
            var response = await id.Delete($"/users/{other.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}