using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.IntegrationTests
{
    public class CompanyUserControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task AddUser_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Put($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUser_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddUser_MissingUser_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Put($"/companies/{company.Id}/users/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUser_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            var members = await id.Get($"/companies/{company.Id}/users").AsAsync<UserPublicView[]>();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
            members.Has(1);
            var member = members[0];
            member.Id.IsEqual(user.Id);
        }

        [Fact]
        public async Task AddUserRole_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUserRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddUserRole_MissingUser_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUserRole_NotMember_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddUserRole_MissingRole_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUserRole_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            var role = await id.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new CompanyRolePayload { Key = "one", Name = "Role One" }).AsAsync<CompanyRoleView>();

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/roles/{role.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUserRole_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUserRole_MissingUser_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{Guid.NewGuid()}/roles/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserRole_NotMember_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUserRole_MissingRole_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/roles/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserRole_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            var role = await id.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new CompanyRolePayload { Key = "one", Name = "Role One" }).AsAsync<CompanyRoleView>();
            await id.Put($"/companies/{company.Id}/users/{user.Id}/roles/{role.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/roles/{role.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AddUserClaim_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "S" };

            // act
            var response = await id.Post($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}")
                .BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddUserClaim_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}")
                .BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUserClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/claims/{Guid.NewGuid()}")
                .BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddUserClaim_MissingUser_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}")
                .BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUserClaim_NotMember_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/claims/{Guid.NewGuid()}")
                .BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddUserClaim_MissingClaim_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/claims/{Guid.NewGuid()}")
                .BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddUserClaim_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            var claim = await id.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new CompanyClaimPayload { Key = "one", Name = "Claim One" }).AsAsync<CompanyClaimView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/companies/{company.Id}/users/{user.Id}/claims/{claim.Id}")
                .BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUserClaim_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUserClaim_MissingUser_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserClaim_NotMember_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUserClaim_MissingClaim_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserClaim_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            await id.Put($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            var claim = await id.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new CompanyClaimPayload { Key = "one", Name = "Claim One" }).AsAsync<CompanyClaimView>();
            await id.Put($"/companies/{company.Id}/users/{user.Id}/claims/{claim.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}/claims/{claim.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUser_MissingCompany_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{Guid.NewGuid()}/users/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUser_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUser_MissingUser_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUser_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var company = await CreateCompanyAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/companies/{company.Id}/users/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}