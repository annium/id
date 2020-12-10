using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.TestClient;
using Annium.Id.Api.ViewModels.Requests.Claims;
using Annium.Testing;
using Xunit;

namespace Annium.Id.Api.IntegrationTests.Controllers
{
    public class ClaimControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new CreateClaimRequest { AppId = Guid.Empty, Key = "one" };

            // act
            var response = await Id(token).Claim.CreateClaim(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new CreateClaimRequest { AppId = Guid.NewGuid(), Key = "one", Name = "First Claim" };

            // act
            var response = await Id(token).Claim.CreateClaim(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var app = await Id(otherToken).App.Register();
            var token = await Id().RegisterLogUserIn();
            var request = new CreateClaimRequest { AppId = app.Id, Key = "one", Name = "First Claim" };

            // act
            var response = await Id(token).Claim.CreateClaim(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NonUniqueKey_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var request = new CreateClaimRequest { AppId = app.Id, Key = "one", Name = "First Claim" };

            await Id(token).Claim.CreateClaim(request);

            // act
            var response = await Id(token).Claim.CreateClaim(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var claimKey = "first";
            var claimName = "First claim";

            // act
            var claim = await Id(token).Claim.Register(app.Id, claimKey, claimName);

            // assert
            claim.Id.IsNotDefault();
            claim.IsEqual(new { AppId = app.Id, Key = claimKey, Name = claimName });
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Claim.ListClaims(Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var claim = await Id(token).Claim.Register(app.Id);

            // act
            var claims = await Id(token).Claim.ListClaims(app.Id).GetData();

            // assert
            claims.Has(1);
            claims.At(0).Id.IsEqual(claim.Id);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var claim = await Id(token).Claim.Register(app.Id);
            var request = new UpdateClaimRequestBody { Key = "one" };

            // act
            var response = await Id(token).Claim.UpdateClaim(claim.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var app = await Id(otherToken).App.Register();
            var claim = await Id(otherToken).Claim.Register(app.Id);
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateClaimRequestBody { Key = "one", Name = "One Claim" };

            // act
            var response = await Id(token).Claim.UpdateClaim(claim.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingClaim_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateClaimRequestBody { Key = "one", Name = "One Claim" };

            // act
            var response = await Id(token).Claim.UpdateClaim(Guid.NewGuid(), request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var claim = await Id(token).Claim.Register(app.Id);
            var request = new UpdateClaimRequestBody { Key = "other", Name = "One Claim" };
            await Id(token).Claim.Register(app.Id, request.Key, request.Name);

            // act
            var response = await Id(token).Claim.UpdateClaim(claim.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var claim = await Id(token).Claim.Register(app.Id);
            var request = new UpdateClaimRequestBody { Key = "one", Name = "One Claim" };

            // act
            var response = await Id(token).Claim.UpdateClaim(claim.Id, request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var otherToken = await Id().RegisterLogOtherUserIn();
            var app = await Id(otherToken).App.Register();
            var claim = await Id(otherToken).Claim.Register(app.Id);
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Claim.DeleteClaim(claim.Id);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_MissingClaim_NotFound()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Claim.DeleteClaim(Guid.NewGuid());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var app = await Id(token).App.Register();
            var claim = await Id(token).Claim.Register(app.Id);

            // act
            var response = await Id(token).Claim.DeleteClaim(claim.Id);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}