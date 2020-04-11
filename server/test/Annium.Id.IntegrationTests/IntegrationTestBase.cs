using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.IntegrationTesting;
using Annium.Core.DependencyInjection;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Email.Models;
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
using Annium.Net.Mail;
using Annium.Testing;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.IntegrationTests
{
    public class IntegrationTestBase : IntegrationTest
    {
        protected IHttpRequest id => GetRequest<Api.Startup>(
            builder => builder.UseServicePack<Api.TestServicePack>(),
            services => services.AddSingleton<IEmailService>(emailService)
        );

        protected IHttpRequest demo(Guid appId) => GetRequest<Id.DemoClient.Startup>(
            builder => builder.UseServicePack<Id.DemoClient.ServicePack>(),
            (IServiceCollection services) => services
                .AddIdAuthorization(options =>
                {
                    options.Audience = appId;
                    options.PublicKeyFile = Path.Combine("keys", "public.key");
                    options.AccessTokenLifeTime = Duration.FromMinutes(5);
                    options.RefreshTokenLifeTime = Duration.FromMinutes(5);
                })
        );

        protected TestEmailService emailService = new TestEmailService();

        private async Task CreateUserAsync(
            string login = "demo",
            string email = "demo@demo.com"
        )
        {
            var createUserRequest = new RegisterMeRequest { Server = "http://localhost/", Login = login, Email = email };
            await id.Post("/me")
                .JsonContent(createUserRequest)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>();
        }

        protected async Task<MeResponse> RegisterLogUserInGetUserAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            var tokens = await RegisterLogUserInAsync(login, password, email);

            return await GetUserAsync(tokens.AccessToken);
        }

        protected Task<TokensResponse> LogUserInAsync(
            string login = "demo",
            string password = "testtest"
        )
        {
            return LogUserInAppAsync(Constants.IdAppId, login, password);
        }

        protected async Task<TokensResponse> LogUserInAppAsync(
            Guid appId,
            string login = "demo",
            string password = "testtest"
        )
        {
            var logUserInRequest = new LogInRequest { Login = login, Password = password };

            var token = (await id.Post($"/me/{appId}/login")
                .JsonContent(logUserInRequest)
                .EnsureSuccessStatusCode()
                .AsResultAsync<TokensResponse>()).Data;

            return token;
        }

        protected Task<TokensResponse> RegisterLogUserInAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            return RegisterLogUserInAppAsync(Constants.IdAppId, login, password, email);
        }

        protected async Task<TokensResponse> RegisterLogUserInAppAsync(
            Guid appId,
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com"
        )
        {
            await CreateUserAsync(login, email);

            // get id from email data
            var userId = emailService.Emails.Last().Data.As<ConfirmEmailData>().Id;

            // confirm email
            var token = (await id.Post($"/me/{appId}/confirm-email")
                .JsonContent(new ConfirmMyEmailRequestBase { Id = userId })
                .EnsureSuccessStatusCode()
                .AsResultAsync<TokensResponse>()).Data;

            // set password
            await id.Put("/me/password")
                .BearerAuthorization(token.AccessToken)
                .JsonContent(new UpdateMyPasswordRequest { Password = password })
                .EnsureSuccessStatusCode()
                .RunAsync();

            // perform regular login
            token = (await id.Post($"/me/{appId}/login")
                .JsonContent(new LogInRequest { Login = login, Password = password })
                .EnsureSuccessStatusCode()
                .AsResultAsync<TokensResponse>()).Data;

            return token;
        }

        protected async Task<MeResponse> GetUserAsync(
            string accessToken
        )
        {
            var user = (await id.Get("/me")
                .BearerAuthorization(accessToken)
                .EnsureSuccessStatusCode()
                .AsResultAsync<MeResponse>()).Data;

            return user;
        }

        protected async Task<AppResponse> CreateAppAsync(
            string accessToken,
            string name = "Demo App"
        )
        {
            var request = new CreateAppRequest { Name = name };

            var appId = (await id.Post("/apps")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>()).Data;

            return (await id.Get($"/apps/{appId}")
                .BearerAuthorization(accessToken)
                .EnsureSuccessStatusCode()
                .AsResultAsync<AppResponse>()).Data;
        }

        protected async Task<ValueTuple<MeResponse, AppResponse, TokensResponse>> RegisterLogUserInCreateAppLogInAppAsync(
            string login = "demo",
            string password = "testtest",
            string email = "demo@demo.com",
            string appName = "Demo App"
        )
        {
            var tokens = await RegisterLogUserInAsync(login, password, email);
            var user = await GetUserAsync(tokens.AccessToken);
            var app = await CreateAppAsync(tokens.AccessToken, appName);

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

            var roleId = (await id.Post("/roles")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>()).Data;

            var role = (await id.Get("/roles")
                    .BearerAuthorization(accessToken)
                    .Param("appId", appId)
                    .EnsureSuccessStatusCode()
                    .AsResultAsync<RoleResponse[]>()).Data
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

            var claimId = (await id.Post("/claims")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>()).Data;

            var claim = (await id.Get("/claims")
                    .BearerAuthorization(accessToken)
                    .Param("appId", appId)
                    .EnsureSuccessStatusCode()
                    .AsResultAsync<ClaimResponse[]>()).Data
                .First(c => c.Id == claimId);

            return claim;
        }

        protected Task<IHttpResponse> AddClaimToRoleAsync(
            string accessToken,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddClaimToRoleRequest { Value = value };

            return id.Post($"/roles/{roleId}/claims/{claimId}")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .RunAsync();
        }

        protected Task AddRoleToUserAsync(
            string accessToken,
            Guid userId,
            Guid roleId
        )
        {
            return id.Post($"/users/{userId}/roles/{roleId}")
                .BearerAuthorization(accessToken)
                .EnsureSuccessStatusCode()
                .RunAsync();
        }

        protected Task AddClaimToUserAsync(
            string accessToken,
            Guid userId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddClaimToUserRequest { Value = value };

            return id.Post($"/users/{userId}/claims/{claimId}")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .RunAsync();
        }

        protected async Task<CompanyResponse> CreateCompanyAsync(
            string accessToken,
            string name = "Demo Company",
            Guid? parentId = null
        )
        {
            var request = new RegisterCompanyRequest { ParentId = parentId, Name = name };

            var companyId = (await id.Post("/companies")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>()).Data;

            return (await id.Get($"/companies/{companyId}")
                .BearerAuthorization(accessToken)
                .EnsureSuccessStatusCode()
                .AsResultAsync<CompanyResponse>()).Data;
        }

        protected async Task<CompanyRoleResponse> CreateCompanyRoleAsync(
            string accessToken,
            Guid appId,
            string key = "one",
            string name = "First role"
        )
        {
            var request = new CreateCompanyRoleRequest { AppId = appId, Key = key, Name = name };

            var roleId = (await id.Post("/companies/roles")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>()).Data;

            var role = (await id.Get("/companies/roles")
                    .BearerAuthorization(accessToken)
                    .Param("appId", appId)
                    .EnsureSuccessStatusCode()
                    .AsResultAsync<CompanyRoleResponse[]>()).Data
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

            var claimId = (await id.Post("/companies/claims")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .AsResultAsync<Guid>()).Data;

            var claim = (await id.Get("/companies/claims")
                    .BearerAuthorization(accessToken)
                    .Param("appId", appId)
                    .EnsureSuccessStatusCode()
                    .AsResultAsync<CompanyClaimResponse[]>()).Data
                .First(c => c.Id == claimId);

            return claim;
        }

        protected Task<IHttpResponse> AddCompanyClaimToCompanyRoleAsync(
            string accessToken,
            Guid roleId,
            Guid claimId,
            string value = "Some"
        )
        {
            var request = new AddCompanyClaimToCompanyRoleRequest { Value = value };

            return id.Post($"/companies/roles/{roleId}/claims/{claimId}")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .RunAsync();
        }

        protected Task AddUserToCompanyAsync(
            string accessToken,
            Guid companyId,
            Guid userId
        )
        {
            return id.Post($"/companies/{companyId}/users/{userId}")
                .BearerAuthorization(accessToken)
                .EnsureSuccessStatusCode()
                .RunAsync();
        }

        protected Task AddCompanyRoleToCompanyUserAsync(
            string accessToken,
            Guid companyId,
            Guid userId,
            Guid roleId
        )
        {
            return id.Post($"/companies/{companyId}/users/{userId}/roles/{roleId}")
                .BearerAuthorization(accessToken)
                .EnsureSuccessStatusCode()
                .RunAsync();
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

            return id.Post($"/companies/{companyId}/users/{userId}/claims/{claimId}")
                .BearerAuthorization(accessToken)
                .JsonContent(request)
                .EnsureSuccessStatusCode()
                .RunAsync();
        }
    }
}