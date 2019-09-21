using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Id.ViewModels.Apps.Requests;
using Annium.Id.ViewModels.Apps.Responses;
using Annium.Id.ViewModels.AppUsers.Requests;
using Annium.Id.ViewModels.Claims.Requests;
using Annium.Id.ViewModels.Claims.Responses;
using Annium.Id.ViewModels.Companies.Requests;
using Annium.Id.ViewModels.Companies.Responses;
using Annium.Id.ViewModels.CompanyClaims.Requests;
using Annium.Id.ViewModels.CompanyClaims.Responses;
using Annium.Id.ViewModels.CompanyRoles.Requests;
using Annium.Id.ViewModels.CompanyRoles.Responses;
using Annium.Id.ViewModels.CompanyUsers.Requests;
using Annium.Id.ViewModels.Roles.Requests;
using Annium.Id.ViewModels.Roles.Responses;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Id.ViewModels.Users.Responses;
using Annium.Net.Http;

namespace Annium.Id.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        protected IRequest id => GetRequest<Api.Startup<Api.TestServicePack>>();
        protected IRequest demo => GetRequest<Id.DemoClient.Startup<Id.DemoClient.ServicePack>>();

        protected async Task<UserPrivateResponse> RegisterUserAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var(user, _) = await LogUserInAsync(login, password, email);

            return user;
        }

        protected async Task<ValueTuple<UserPrivateResponse, UserTokenResponse>> LogUserInAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var createUserRequest = new CreateUserRequest { Login = login, Password = password, Email = email };
            await id.Post("/me").JsonContent(createUserRequest).AsResultAsync<Guid>();

            var logUserInRequest = new LogUserInRequest { Login = login, Password = password };

            var token = (await id.Post("/me/login").JsonContent(logUserInRequest).AsResultAsync<UserTokenResponse>()).Data;

            var user = (await id.Get("/me").BearerAuthorization(token.AccessToken).AsResultAsync<UserPrivateResponse>()).Data;

            return (user, token);
        }

        protected async Task<AppPublicResponse> CreateAppAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo App"
        )
        {
            var request = new CreateAppRequest { Key = key, Name = name };

            var appId = (await id.Post("/apps").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            return (await id.Get($"/apps/{appId}").BearerAuthorization(accessToken).AsResultAsync<AppPublicResponse>()).Data;
        }

        protected async Task<UserTokenResponse> LogUserInAppAsync(
            Guid appId,
            string login = "demo",
            string password = "testtest"
        )
        {
            var request = new LogUserInAppRequest { Login = login, Password = password };

            var tokenResult = await id.Post($"/me/apps/{appId}/login").JsonContent(request).AsResultAsync<UserTokenResponse>();

            return tokenResult.Data;
        }

        protected async Task<ValueTuple<UserPrivateResponse, AppPublicResponse, UserTokenResponse>> LogUserInCreateAppLoginAppAsync(
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

            var appTokens = (await id.Post($"/me/apps/{app.Id}/login").JsonContent(request).AsResultAsync<UserTokenResponse>()).Data;

            return (user, app, appTokens);
        }

        protected async Task<RoleResponse> CreateRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateRoleRequest { Key = key, Name = name };

            var roleId = (await id.Post($"/apps/{appId}/roles").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var role = (await id.Get($"/apps/{appId}/roles").BearerAuthorization(accessToken).AsResultAsync<RoleResponse[]>()).Data
                .First(c => c.Id == roleId);

            return role;
        }

        protected async Task<ClaimResponse> CreateClaimAsync(
            string accessToken,
            Guid appId,
            string key = "first",
            string name = "First claim"
        )
        {
            var request = new CreateClaimRequest { Key = key, Name = name };

            var claimId = (await id.Post($"/apps/{appId}/claims").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var claim = (await id.Get($"/apps/{appId}/claims").BearerAuthorization(accessToken).AsResultAsync<ClaimResponse[]>()).Data
                .First(c => c.Id == claimId);

            return claim;
        }

        protected async Task<ClaimValueResponse> AddClaimToRoleAsync(
            string accessToken,
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddClaimToRoleRequest { Value = value };

            var response = await id.Post($"/apps/{appId}/roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
            if (!response.IsSuccessStatusCode)
                return null;

            var role = (await id.Get($"/apps/{appId}/roles").BearerAuthorization(accessToken).AsResultAsync<RoleResponse[]>()).Data
                .First(c => c.Id == roleId);

            // perhaps, unstable, due value is not unique
            return role.Claims.First(c => c.Value == value);
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

        protected Task AddClaimToUserAsync(
            string accessToken,
            Guid appId,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddClaimToUserRequest { Value = value };

            return id.Post($"/apps/{appId}/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
        }

        protected async Task<CompanyResponse> CreateCompanyAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo Company",
            Guid? parentId = null
        )
        {
            var request = new RegisterCompanyRequest { ParentId = parentId, Key = key, Name = name };

            var companyId = (await id.Post("/companies").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;
            Console.WriteLine(companyId);

            return (await id.Get($"/companies/{companyId}").BearerAuthorization(accessToken).AsResultAsync<CompanyResponse>()).Data;
        }

        protected async Task<CompanyRoleResponse> CreateCompanyRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateCompanyRoleRequest { Key = key, Name = name };

            var roleId = (await id.Post($"/apps/{appId}/company-roles").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var role = (await id.Get($"/apps/{appId}/company-roles").BearerAuthorization(accessToken).AsResultAsync<CompanyRoleResponse[]>()).Data
                .First(c => c.Id == roleId);

            return role;
        }

        protected async Task<CompanyClaimResponse> CreateCompanyClaimAsync(
            string accessToken,
            Guid appId,
            string key = "first",
            string name = "First claim"
        )
        {
            var request = new CreateCompanyClaimRequest { Key = key, Name = name };

            var claimId = (await id.Post($"/apps/{appId}/company-claims").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var claim = (await id.Get($"/apps/{appId}/company-claims").BearerAuthorization(accessToken).AsResultAsync<CompanyClaimResponse[]>()).Data
                .First(c => c.Id == claimId);

            return claim;
        }

        protected async Task<CompanyClaimValueResponse> AddCompanyClaimToCompanyRoleAsync(
            string accessToken,
            Guid appId,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddCompanyClaimToCompanyRoleRequest { Value = value };

            var response = await id.Post($"/apps/{appId}/company-roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
            if (!response.IsSuccessStatusCode)
                return null;

            var role = (await id.Get($"/apps/{appId}/company-roles").BearerAuthorization(accessToken).AsResultAsync<CompanyRoleResponse[]>()).Data
                .First(c => c.Id == roleId);

            // perhaps, unstable, due value is not unique
            return role.Claims.First(c => c.Value == value);
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
            var request = new AddCompanyClaimToCompanyUserRequest { Value = value };

            return id.Post($"/companies/{companyId}/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
        }
    }
}