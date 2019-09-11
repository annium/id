using System.Net;
using System.Threading.Tasks;
using Annium.Net.Http;
using Annium.Testing;
using Newtonsoft.Json;
using Annium.Id.Core;

namespace Annium.Id.IntegrationTests.DemoClient.Controllers
{
    public class DemoControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task BaseIdAuthorization_Unauthorized_ReturnsUnauthorized()
        {
            // act
            var response = await demo.Get("/base").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task BaseIdAuthorization_Authorized_Works()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await demo.Get("/base").BearerAuthorization(tokens.AccessToken).RunAsync();
            var token = JsonConvert.DeserializeObject<IdBaseToken>(await response.Content.ReadAsStringAsync());

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            token.IsNotDefault();
            token.UserId.IsEqual(user.Id);
            token.LoginId.IsNotDefault();
        }

        [Fact]
        public async Task AppIdAuthorization_CheckRole_Unauthorized_ReturnsUnauthorized()
        {
            // act
            var response = await demo.Get("/isAdmin").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task AppIdAuthorization_CheckRole_HasNoAccess_ReturnsForbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await demo.Get("/isAdmin").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AppIdAuthorization_CheckRole_HasAccess_Works()
        {
            // arrange
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, "admin", "Administrator");
            await AddRoleToUserAsync(tokens.AccessToken, app.Id, user.Id, role.Id);
            tokens = await LoginAppAsync(app.Id);

            // act
            var response = await demo.Get("/isAdmin").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AppIdAuthorization_CheckClaim_HasRoleAccess_Works()
        {
            // arrange
            // for test completeness -
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, "coo", "Chief Operations Officer");
            await AddRoleToUserAsync(tokens.AccessToken, app.Id, user.Id, role.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, "paymentsAccess", "Payments Access");
            await AddClaimToRoleAsync(tokens.AccessToken, app.Id, role.Id, claim.Id, "full");
            tokens = await LoginAppAsync(app.Id);

            // act
            var response = await demo.Get("/hasPaymentsAccess/").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AppIdAuthorization_CheckClaim_HasClaimAccess_Works()
        {
            // arrange
            // for test completeness -
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, "paymentsAccess", "Payments Access");
            await AddClaimToUserAsync(tokens.AccessToken, app.Id, user.Id, claim.Id, "full");
            tokens = await LoginAppAsync(app.Id);

            // act
            var response = await demo.Get("/hasPaymentsAccess/").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AppIdAuthorization_CheckClaim_HasRoleClaimAccess_Works()
        {
            // arrange
            // for test completeness -
            var(user, app, tokens) = await LogUserInCreateAppLoginAppAsync();
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, "coo", "Chief Operations Officer");
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id, "paymentsAccess", "Payments Access");
            await AddClaimToRoleAsync(tokens.AccessToken, app.Id, role.Id, claim.Id, "limited");
            await AddClaimToUserAsync(tokens.AccessToken, app.Id, user.Id, claim.Id, "full");
            tokens = await LoginAppAsync(app.Id);

            // act
            var response = await demo.Get("/hasPaymentsAccess/").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}