using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.DemoClient.Controllers
{
    public class DemoControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task IdAuthorization_Unauthorized_ReturnsUnauthorized()
        {
            // act
            var response = await demo(Guid.NewGuid()).Get("/base").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task IdAuthorization_Authorized_Works()
        {
            // arrange
            var (_, app, _) = await RegisterLogUserInCreateAppLogInAppAsync();
            var appTokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await demo(app.Id).Get("/base").BearerAuthorization(appTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            // FIXME: use, when System.Text.Json supports Deserialize with non-default constructor
            // var token = await demo(app.Id).Get("/base").BearerAuthorization(appTokens.AccessToken).AsAsync<IdToken>();

            // // assert
            // token.IsNotDefault();
            // token.UserId.IsEqual(user.Id);
            // token.LoginId.IsNotDefault();
        }

        [Fact]
        public async Task IdAuthorization_CheckRole_Unauthorized_ReturnsUnauthorized()
        {
            // act
            var response = await demo(Guid.NewGuid()).Get("/isAdmin").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task IdAuthorization_CheckRole_HasNoAccess_ReturnsForbidden()
        {
            // arrange
            var (_, app, _) = await RegisterLogUserInCreateAppLogInAppAsync();
            var appTokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await demo(app.Id).Get("/isAdmin").BearerAuthorization(appTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task IdAuthorization_CheckRole_HasAccess_Works()
        {
            // arrange
            var (user, app, tokens) = await RegisterLogUserInCreateAppLogInAppAsync();
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, "admin", "Administrator");
            await AddRoleToUserAsync(tokens.AccessToken, user.Id, role.Id);
            var appTokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await demo(app.Id).Get("/isAdmin").BearerAuthorization(appTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task IdAuthorization_CheckClaim_HasRoleAccess_Works()
        {
            // arrange
            // for test completeness -
            var (user, app, tokens) = await RegisterLogUserInCreateAppLogInAppAsync();
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, "coo", "Chief Operations Officer");
            await AddRoleToUserAsync(tokens.AccessToken, user.Id, role.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, "paymentsAccess", "Payments Access");
            await AddClaimToRoleAsync(tokens.AccessToken, role.Id, claim.Id, "full");
            var appTokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await demo(app.Id).Get("/hasPaymentsAccess/").BearerAuthorization(appTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task IdAuthorization_CheckClaim_HasClaimAccess_Works()
        {
            // arrange
            // for test completeness -
            var (user, app, tokens) = await RegisterLogUserInCreateAppLogInAppAsync();
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, "paymentsAccess", "Payments Access");
            await AddClaimToUserAsync(tokens.AccessToken, user.Id, claim.Id, "full");
            var appTokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await demo(app.Id).Get("/hasPaymentsAccess/").BearerAuthorization(appTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task IdAuthorization_CheckClaim_HasRoleClaimAccess_Works()
        {
            // arrange
            // for test completeness -
            var (user, app, tokens) = await RegisterLogUserInCreateAppLogInAppAsync();
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, "coo", "Chief Operations Officer");
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, "paymentsAccess", "Payments Access");
            await AddClaimToRoleAsync(tokens.AccessToken, role.Id, claim.Id, "limited");
            await AddClaimToUserAsync(tokens.AccessToken, user.Id, claim.Id, "full");
            var appTokens = await LogUserInAppAsync(app.Id);

            // act
            var response = await demo(app.Id).Get("/hasPaymentsAccess/").BearerAuthorization(appTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}