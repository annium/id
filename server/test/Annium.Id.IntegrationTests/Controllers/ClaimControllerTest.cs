using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class ClaimControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var p = new ClaimPayload() { Key = "one" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var p = new ClaimPayload() { Key = "one", Name = "First Claim" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LogUserInAsync();
            var p = new ClaimPayload() { Key = "one", Name = "First Claim" };

            // act
            var response = await id.Post($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NonUniqueKey_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new ClaimPayload() { Key = "one", Name = "First Claim" };

            await id.Post($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // act
            var response = await id.Post($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claimKey = "first";
            var claimName = "First claim";

            // act
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, claimKey, claimName);

            // assert
            claim.Id.IsNotDefault();
            claim.AppId.IsEqual(app.Id);
            claim.Key.IsEqual(claimKey);
            claim.Name.IsEqual(claimName);
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Get($"/apps/{Guid.NewGuid()}/claims").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);

            // act
            var claims = (await id.Get($"/apps/{app.Id}/claims").BearerAuthorization(tokens.AccessToken).AsResultAsync<ClaimView[]>()).Data;

            // assert
            claims.Has(1);
            claims[0].Id.IsEqual(claim.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var u = new ClaimPayload { Key = "one" };

            // act
            var response = await id.Put($"/apps/{app.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var u = new ClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/apps/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var claim = await CreateClaimAsync(ownerTokens.AccessToken, app.Id);
            var(user, tokens) = await LogUserInAsync();
            var u = new ClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/apps/{app.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var u = new ClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/apps/{app.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var claim = await CreateClaimAsync(tokens.AccessToken, app1.Id);
            var u = new ClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/apps/{app2.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var u = new ClaimPayload { Key = "other", Name = "One Claim" };
            await CreateClaimAsync(tokens.AccessToken, app.Id, u.Key, u.Name);

            // act
            var response = await id.Put($"/apps/{app.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var u = new ClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/apps/{app.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var claim = await CreateClaimAsync(ownerTokens.AccessToken, app.Id);
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await id.Delete($"/apps/{app.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ClaimBelongsOtherApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var claim = await CreateClaimAsync(tokens.AccessToken, app1.Id);

            // act
            var response = await id.Delete($"/apps/{app2.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/apps/{app.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}