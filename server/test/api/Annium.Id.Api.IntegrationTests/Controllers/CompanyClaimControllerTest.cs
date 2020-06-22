using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.ViewModels.CompanyClaims.Requests;
using Annium.Id.Api.ViewModels.CompanyClaims.Responses;
using Annium.Net.Http;
using Annium.Testing;
using Xunit;

namespace Annium.Id.Api.IntegrationTests.Controllers
{
    public class CompanyClaimControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateCompanyClaimRequest() { AppId = Guid.NewGuid(), Key = "one" };

            // act
            var response = await id.Post("/companies/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateCompanyClaimRequest() { AppId = Guid.NewGuid(), Key = "one", Name = "First Claim" };

            // act
            var response = await id.Post("/companies/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NonUniqueKey_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new CreateCompanyClaimRequest() { AppId = app.Id, Key = "one", Name = "First Claim" };

            await id.Post("/companies/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // act
            var response = await id.Post("/companies/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateCompanyClaimRequest() { AppId = app.Id, Key = "one", Name = "First Claim" };

            // act
            var response = await id.Post("/companies/claims").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claimKey = "first";
            var claimName = "First claim";

            // act
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id, claimKey, claimName);

            // assert
            claim.Id.IsNotDefault();
            claim.IsEqual(new { AppId = app.Id, Key = claimKey, Name = claimName });
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Get("/companies/claims").BearerAuthorization(tokens.AccessToken).Param("appId", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);

            // act
            var claims = (await id.Get("/companies/claims").BearerAuthorization(tokens.AccessToken).Param("appId", app.Id)
                .AsResultAsync<CompanyClaimResponse[]>()).Data;

            // assert
            claims.Has(1);
            claims[0].Id.IsEqual(claim.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyClaimRequest { Key = "one" };

            // act
            var response = await id.Put($"/companies/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingClaim_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyClaimRequest { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/companies/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var claim = await CreateCompanyClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyClaimRequest { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/companies/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);
            var u = new UpdateCompanyClaimRequest { Key = "other", Name = "One Claim" };
            await CreateCompanyClaimAsync(tokens.AccessToken, app.Id, u.Key, u.Name);

            // act
            var response = await id.Put($"/companies/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);
            var u = new UpdateCompanyClaimRequest { Key = "one", Name = "One Claim" };

            // act
            var response = await id.Put($"/companies/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_MissingClaim_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var claim = await CreateCompanyClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/companies/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}