using System;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;

namespace Annium.Id.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        protected IRequest id => GetRequest<Api.Startup<Api.TestServicePack>>();
        protected IRequest demo => GetRequest<Annium.Id.DemoClient.Startup<Annium.Id.DemoClient.ServicePack>>();

        protected async Task<UserPrivateView> RegisterUserAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var payload = new UserPayload { Login = login, Password = password, Email = email };

            return await id.Put("/me").JsonContent(payload).AsAsync<UserPrivateView>();
        }

        protected async Task<ValueTuple<UserPrivateView, UserTokenView>> LoginUserAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var user = await RegisterUserAsync(login, password, email);
            var payload = new UserLoginPayload { Login = login, Password = password };

            var tokens = await id.Post("/me/login").JsonContent(payload).AsAsync<UserTokenView>();

            return (user, tokens);
        }

        protected Task<AppPrivateView> CreateAppAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo App"
        )
        {
            var payload = new AppPayload { Key = key, Name = name };

            return id.Put("/apps").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<AppPrivateView>();
        }

        protected Task<UserTokenView> LoginAppAsync(
            Guid appId,
            string login = "demo",
            string password = "testtest"
        )
        {
            var payload = new UserLoginPayload { Login = login, Password = password };

            return id.Post($"/me/apps/{appId}/login").JsonContent(payload).AsAsync<UserTokenView>();
        }

        protected async Task<ValueTuple<UserPrivateView, AppPrivateView, UserTokenView>> LoginUserCreateAppLoginAppAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com",
            string appKey = "demo",
            string appName = "Demo App"
        )
        {
            var(user, tokens) = await LoginUserAsync(login, password, email);
            var app = await CreateAppAsync(tokens.AccessToken);
            var payload = new UserLoginPayload { Login = login, Password = password };

            tokens = await id.Post($"/me/apps/{app.Id}/login").JsonContent(payload).AsAsync<UserTokenView>();

            return (user, app, tokens);
        }

        protected Task<RoleView> CreateRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var payload = new RolePayload { Key = key, Name = name };

            return id.Put($"/apps/{appId}/roles").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<RoleView>();
        }

        protected Task<ClaimView> CreateClaimAsync(
            string accessToken,
            Guid appId,
            string key = "first",
            string name = "First claim"
        )
        {
            var payload = new ClaimPayload { Key = key, Name = name };

            return id.Put($"/apps/{appId}/claims").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<ClaimView>();
        }

        protected Task<ClaimValueView> AddClaimToRoleAsync(
            string accessToken,
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var payload = new ClaimValuePayload { Value = value };

            return id.Post($"/apps/{appId}/roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<ClaimValueView>();
        }

        protected Task AddRoleToUserAsync(
            string accessToken,
            Guid appId,
            Guid userId,
            Guid roleId
        )
        {
            return id.Put($"/apps/{appId}/users/{userId}/roles/{roleId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task<ClaimValueView> AddClaimToUserAsync(
            string accessToken,
            Guid appId,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var payload = new ClaimValuePayload { Value = value };

            return id.Post($"/apps/{appId}/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<ClaimValueView>();
        }

        protected Task<CompanyPrivateView> CreateCompanyAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo Company",
            Guid? parentId = null
        )
        {
            var payload = new CompanyPayload { ParentId = parentId, Key = key, Name = name };

            return id.Put("/companies").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<CompanyPrivateView>();
        }

        protected Task<RoleView> CreateCompanyRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var payload = new RolePayload { Key = key, Name = name };

            return id.Put($"/apps/{appId}/company-roles").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<RoleView>();
        }

        protected Task<CompanyClaimView> CreateCompanyClaimAsync(
            string accessToken,
            Guid appId,
            string key = "first",
            string name = "First claim"
        )
        {
            var payload = new CompanyClaimPayload { Key = key, Name = name };

            return id.Put($"/apps/{appId}/company-claims").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<CompanyClaimView>();
        }

        protected Task<ClaimValueView> AddCompanyClaimToCompanyRoleAsync(
            string accessToken,
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var payload = new ClaimValuePayload { Value = value };

            return id.Post($"/apps/{appId}/company-roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<ClaimValueView>();
        }

        protected Task AddUserToCompanyAsync(
            string accessToken,
            Guid companyId,
            Guid userId
        )
        {
            return id.Put($"/companies/{companyId}/users/{userId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task AddCompanyRoleToCompanyUserAsync(
            string accessToken,
            Guid companyId,
            Guid userId,
            Guid roleId
        )
        {
            return id.Put($"/companies/{companyId}/users/{userId}/roles/{roleId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task AddCompanyClaimToCompanyUserAsync(
            string accessToken,
            Guid companyId,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var payload = new ClaimValuePayload { Value = value };

            return id.Post($"/companies/{companyId}/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(payload).AsAsync<ClaimValueView>();
        }
    }
}