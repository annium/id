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
    public class CompanyClaimControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new CompanyClaimPayload() { Key = "one" };

            // act
            var response = await http.Put($"/apps/{Guid.NewGuid()}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };

            // act
            var response = await http.Put($"/apps/{Guid.NewGuid()}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };

            // act
            var response = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };

            await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // act
            var response = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };

            // act
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();

            // assert
            claim.Id.IsNotDefault();
            claim.AppId.IsEqual(app.Id);
            claim.Key.IsEqual(p.Key);
            claim.Name.IsEqual(p.Name);
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Get($"/apps/{Guid.NewGuid()}/company-claims").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();

            // act
            var claims = await http.Get($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).AsAsync<CompanyClaimView[]>();

            // assert
            claims.Has(1);
            claims[0].Id.IsEqual(claim.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var u = new CompanyClaimPayload { Key = "one" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var u = new CompanyClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}/company-claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var(user, tokens) = await LoginAsync();
            var u = new CompanyClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var u = new CompanyClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app1.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var u = new CompanyClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await http.Post($"/apps/{app2.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var u = new CompanyClaimPayload { Key = "other", Name = "One Claim" };
            await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<CompanyClaimView>();

            // act
            var response = await http.Post($"/apps/{app.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();
            var u = new CompanyClaimPayload { Key = "one", Name = "One Claim" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<CompanyClaimView>();

            // assert
            response.Id.IsEqual(claim.Id);
            response.AppId.IsEqual(app.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{Guid.NewGuid()}/company-claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ClaimBelongsOtherApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app1.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();

            // act
            var response = await http.Delete($"/apps/{app2.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CompanyClaimPayload() { Key = "one", Name = "First Claim" };
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyClaimView>();

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}