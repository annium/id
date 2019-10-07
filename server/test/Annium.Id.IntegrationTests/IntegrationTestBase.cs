using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Id.Core;
using Annium.Id.ViewModels.Apps.Requests;
using Annium.Id.ViewModels.Apps.Responses;
using Annium.Id.ViewModels.Claims.Requests;
using Annium.Id.ViewModels.Claims.Responses;
using Annium.Id.ViewModels.Companies.Requests;
using Annium.Id.ViewModels.Companies.Responses;
using Annium.Id.ViewModels.CompanyClaims.Requests;
using Annium.Id.ViewModels.CompanyClaims.Responses;
using Annium.Id.ViewModels.CompanyRoles.Requests;
using Annium.Id.ViewModels.CompanyRoles.Responses;
using Annium.Id.ViewModels.CompanyUsers.Requests;
using Annium.Id.ViewModels.Login.Requests;
using Annium.Id.ViewModels.Login.Responses;
using Annium.Id.ViewModels.Me.Requests;
using Annium.Id.ViewModels.Me.Responses;
using Annium.Id.ViewModels.Roles.Requests;
using Annium.Id.ViewModels.Roles.Responses;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Net.Http;

namespace Annium.Id.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        protected IRequest id => GetRequest<Api.Startup, Api.TestServicePack>();
        protected IRequest demo => GetRequest<Id.DemoClient.Startup, Id.DemoClient.ServicePack>();

        protected async Task<MeResponse> RegisterUserAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var tokens = await LogUserInAsync(login, password, email);

            return await GetUserAsync(tokens.AccessToken);
        }

        protected Task<TokensResponse> LogUserInAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            return LogUserInAppAsync(Constants.IdApp, login, password, email);
        }

        protected async Task<TokensResponse> LogUserInAppAsync(
            string appKey,
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var createUserRequest = new RegisterMeRequest { Login = login, Password = password, Email = email };
            await id.Post("/me").JsonContent(createUserRequest).AsResultAsync<Guid>();

            var logUserInRequest = new LogInRequest { Login = login, Password = password };

            var token = (await id.Post($"/me/{appKey}/login").JsonContent(logUserInRequest).AsResultAsync<TokensResponse>()).Data;

            return token;
        }

        protected async Task<MeResponse> GetUserAsync(
            string accessToken
        )
        {
            var user = (await id.Get("/me").BearerAuthorization(accessToken).AsResultAsync<MeResponse>()).Data;

            return user;
        }

        protected async Task<AppResponse> CreateAppAsync(
            string accessToken,
            string key = "demo",
            string name = "Demo App"
        )
        {
            var request = new CreateAppRequest { Key = key, Name = name };

            var appId = (await id.Post("/apps").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            return (await id.Get($"/apps/{appId}").BearerAuthorization(accessToken).AsResultAsync<AppResponse>()).Data;
        }

        protected async Task<ValueTuple<MeResponse, AppResponse, TokensResponse>> LogUserInCreateAppLogInAppAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com",
            string appKey = "demo",
            string appName = "Demo App"
        )
        {
            var tokens = await LogUserInAsync(login, password, email);
            var user = await GetUserAsync(tokens.AccessToken);
            var app = await CreateAppAsync(tokens.AccessToken, appKey, appName);

            return (user, app, tokens);
        }

        protected async Task<RoleResponse> CreateRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateRoleRequest { AppId = appId, Key = key, Name = name };

            var roleId = (await id.Post("/roles").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var role = (await id.Get("/roles").BearerAuthorization(accessToken).Param("appId", appId).AsResultAsync<RoleResponse[]>()).Data
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
            var request = new CreateClaimRequest { AppId = appId, Key = key, Name = name };

            var claimId = (await id.Post("/claims").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var claim = (await id.Get("/claims").BearerAuthorization(accessToken).Param("appId", appId).AsResultAsync<ClaimResponse[]>()).Data
                .First(c => c.Id == claimId);

            return claim;
        }

        protected Task<IResponse> AddClaimToRoleAsync(
            string accessToken,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddClaimToRoleRequest { Value = value };

            return id.Post($"/roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
        }

        protected Task AddRoleToUserAsync(
            string accessToken,
            Guid userId,
            Guid roleId
        )
        {
            return id.Post($"/users/{userId}/roles/{roleId}").BearerAuthorization(accessToken).RunAsync();
        }

        protected Task AddClaimToUserAsync(
            string accessToken,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddClaimToUserRequest { Value = value };

            return id.Post($"/users/{userId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
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

            return (await id.Get($"/companies/{companyId}").BearerAuthorization(accessToken).AsResultAsync<CompanyResponse>()).Data;
        }

        protected async Task<CompanyRoleResponse> CreateCompanyRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateCompanyRoleRequest { AppId = appId, Key = key, Name = name };

            var roleId = (await id.Post("/companies/roles").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var role = (await id.Get("/companies/roles").BearerAuthorization(accessToken).Param("appId", appId).AsResultAsync<CompanyRoleResponse[]>()).Data
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
            var request = new CreateCompanyClaimRequest { AppId = appId, Key = key, Name = name };

            var claimId = (await id.Post("/companies/claims").BearerAuthorization(accessToken).JsonContent(request).AsResultAsync<Guid>()).Data;

            var claim = (await id.Get("/companies/claims").BearerAuthorization(accessToken).Param("appId", appId).AsResultAsync<CompanyClaimResponse[]>()).Data
                .First(c => c.Id == claimId);

            return claim;
        }

        protected Task<IResponse> AddCompanyClaimToCompanyRoleAsync(
            string accessToken,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddCompanyClaimToCompanyRoleRequest { Value = value };

            return id.Post($"/companies/roles/{roleId}/claims/{claimId}").BearerAuthorization(accessToken).JsonContent(request).RunAsync();
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