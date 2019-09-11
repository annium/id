using System;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Data.Operations;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Id.ViewModels.Users.Responses;
using Annium.Net.Http;

namespace Annium.Id.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        protected IRequest id => GetRequest<Api.Startup<Api.TestServicePack>>();
        protected IRequest demo => GetRequest<Annium.Id.DemoClient.Startup<Annium.Id.DemoClient.ServicePack>>();

        protected async Task<UserPrivateResponse> RegisterUserAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var(user, tokens) = await LogUserInAsync(login, password, email);

            return user;
        }

        protected async Task<ValueTuple<UserPrivateResponse, UserTokenResponse>> LogUserInAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var createUserRequest = new CreateUserRequest { Login = login, Password = password, Email = email };
            await id.Post("/me").JsonContent(createUserRequest).AsAsync<IResult<Guid>>();

            var logUserInRequest = new LogUserInRequest { Login = login, Password = password };

            var tokenResult = await id.Post("/me/login").JsonContent(logUserInRequest).AsAsync<IResult<UserTokenResponse>>();

            var user = await id.Get("/me").BearerAuthorization(tokenResult.Data.AccessToken).AsAsync<IResult<UserPrivateResponse>>();

            return (user.Data, tokenResult.Data);
        }

        protected Task<AppPrivateView> CreateAppAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo App"
        )
        {
            var request = new AppPayload { Key = key, Name = name };

            return id.Post("/apps").BearerAuthorization(accessToken).JsonContent(request).AsAsync<AppPrivateView>();
        }

        protected async Task<UserTokenResponse> LoginAppAsync(
            Guid appId,
            string login = "demo",
            string password = "testtest"
        )
        {
            var request = new LogUserInAppRequest { Login = login, Password = password };

            var tokenResult = await id.Post($"/me/apps/{appId}/login").JsonContent(request).AsAsync<IResult<UserTokenResponse>>();

            return tokenResult.Data;
        }

        protected async Task<ValueTuple<UserPrivateResponse, AppPrivateView, UserTokenResponse>> LogUserInCreateAppLoginAppAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com",
            string appKey = "demo",
            string appName = "Demo App"
        )
        {
            var(user, tokens) = await LogUserInAsync(login, password, email);
            var app = await CreateAppAsync(tokens.AccessToken, appKey, appName);
            var request = new LogUserInRequest { Login = login, Password = password };

            var appTokensResult = await id.Post($"/me/apps/{app.Id}/login").JsonContent(request).AsAsync<IResult<UserTokenResponse>>();

            return (user, app, appTokensResult.Data);
        }

        protected Task<RoleView> CreateRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new RolePayload { Key = key, Name = name };

            return id.Post($"/apps/{appId}/roles").BearerAuthorization(accessToken).JsonContent(request).AsAsync<RoleView>();
        }

        protected Task<ClaimView> CreateClaimAsync(
            string accessToken,
            Guid appId,
            string key = "first",
            string name = "First claim"
        )
        {
            var request = new ClaimPayload { Key = key, Name = name };

            return id.Post($"/apps/{appId}/claims").BearerAuthorization(accessToken).JsonContent(request).AsAsync<ClaimView>();
        }

        protected Task<ClaimValueView> AddClaimToRoleAsync(
            string accessToken,
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new ClaimValuePayload { Value = value };

            return id.Post($"/apps/{appId}/roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).AsAsync<ClaimValueView>();
        }

        protected Task AddRoleToUserAsync(
            string accessToken,
            Guid appId,
            Guid userId,
            Guid roleId
        )
        {
            return id.Post($"/apps/{appId}/users/{userId}/roles/{roleId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task<ClaimValueView> AddClaimToUserAsync(
            string accessToken,
            Guid appId,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new ClaimValuePayload { Value = value };

            return id.Post($"/apps/{appId}/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).AsAsync<ClaimValueView>();
        }

        protected Task<CompanyPrivateView> CreateCompanyAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo Company",
            Guid? parentId = null
        )
        {
            var request = new CompanyPayload { ParentId = parentId, Key = key, Name = name };

            return id.Post("/companies").BearerAuthorization(accessToken).JsonContent(request).AsAsync<CompanyPrivateView>();
        }

        protected Task<RoleView> CreateCompanyRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new RolePayload { Key = key, Name = name };

            return id.Post($"/apps/{appId}/company-roles").BearerAuthorization(accessToken).JsonContent(request).AsAsync<RoleView>();
        }

        protected Task<CompanyClaimView> CreateCompanyClaimAsync(
            string accessToken,
            Guid appId,
            string key = "first",
            string name = "First claim"
        )
        {
            var request = new CompanyClaimPayload { Key = key, Name = name };

            return id.Post($"/apps/{appId}/company-claims").BearerAuthorization(accessToken).JsonContent(request).AsAsync<CompanyClaimView>();
        }

        protected Task<ClaimValueView> AddCompanyClaimToCompanyRoleAsync(
            string accessToken,
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new ClaimValuePayload { Value = value };

            return id.Post($"/apps/{appId}/company-roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).AsAsync<ClaimValueView>();
        }

        protected Task AddUserToCompanyAsync(
            string accessToken,
            Guid companyId,
            Guid userId
        )
        {
            return id.Post($"/companies/{companyId}/users/{userId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task AddCompanyRoleToCompanyUserAsync(
            string accessToken,
            Guid companyId,
            Guid userId,
            Guid roleId
        )
        {
            return id.Post($"/companies/{companyId}/users/{userId}/roles/{roleId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task AddCompanyClaimToCompanyUserAsync(
            string accessToken,
            Guid companyId,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new ClaimValuePayload { Value = value };

            return id.Post($"/companies/{companyId}/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).AsAsync<ClaimValueView>();
        }
    }
}