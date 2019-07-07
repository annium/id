using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Testing;
using Newtonsoft.Json;

namespace Annium.Id.IntegrationTests
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
            var(user, tokens) = await LoginAsync();

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
            var(user, tokens) = await LoginAsync();

            // act
            var response = await demo.Get("/isAdmin").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AppIdAuthorization_CheckRole_HasAccess_Works()
        {
            // arrange
            var(user, app, tokens) = await LoginAppAsync();
            var role = await id.Put($"/apps/{app.Id}/roles")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload { Key = "admin", Name = "Administrator" })
                .AsAsync<RoleView>();
            await id.Put($"apps/{app.Id}/users/{user.Id}/roles/{role.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .RunAsync();
            tokens = await id.Post($"/me/apps/{app.Id}/login")
                .JsonContent(new UserLoginPayload { Login = "demo", Password = "testtest" })
                .AsAsync<UserTokenView>();

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
            var(user, app, tokens) = await LoginAppAsync();
            var role = await id.Put($"/apps/{app.Id}/roles")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload { Key = "coo", Name = "Chief Operations Officer" })
                .AsAsync<RoleView>();
            await id.Put($"apps/{app.Id}/users/{user.Id}/roles/{role.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .RunAsync();
            var claim = await id.Put($"/apps/{app.Id}/claims")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload { Key = "paymentsAccess", Name = "Payments Access" })
                .AsAsync<RoleView>();
            await id.Post($"apps/{app.Id}/roles/{role.Id}/claims/{claim.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimValuePayload { Value = "full" })
                .RunAsync();
            tokens = await id.Post($"/me/apps/{app.Id}/login")
                .JsonContent(new UserLoginPayload { Login = "demo", Password = "testtest" })
                .AsAsync<UserTokenView>();

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
            var(user, app, tokens) = await LoginAppAsync();
            var claim = await id.Put($"/apps/{app.Id}/claims")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload { Key = "paymentsAccess", Name = "Payments Access" })
                .AsAsync<RoleView>();
            await id.Post($"apps/{app.Id}/users/{user.Id}/claims/{claim.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimValuePayload { Value = "full" })
                .RunAsync();
            tokens = await id.Post($"/me/apps/{app.Id}/login")
                .JsonContent(new UserLoginPayload { Login = "demo", Password = "testtest" })
                .AsAsync<UserTokenView>();

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
            var(user, app, tokens) = await LoginAppAsync();
            var role = await id.Put($"/apps/{app.Id}/roles")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload { Key = "operations-officer", Name = "Operations Officer" })
                .AsAsync<RoleView>();
            await id.Put($"apps/{app.Id}/users/{user.Id}/roles/{role.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .RunAsync();
            var claim = await id.Put($"/apps/{app.Id}/claims")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload { Key = "paymentsAccess", Name = "Payments Access" })
                .AsAsync<RoleView>();
            await id.Post($"apps/{app.Id}/roles/{role.Id}/claims/{claim.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimValuePayload { Value = "limited" })
                .RunAsync();
            await id.Post($"apps/{app.Id}/users/{user.Id}/claims/{claim.Id}")
                .BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimValuePayload { Value = "full" })
                .RunAsync();
            tokens = await id.Post($"/me/apps/{app.Id}/login")
                .JsonContent(new UserLoginPayload { Login = "demo", Password = "testtest" })
                .AsAsync<UserTokenView>();

            // act
            var response = await demo.Get("/hasPaymentsAccess/").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}