using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class CompanyRoleControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var p = new RolePayload() { Key = "one" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LogUserInAsync();
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            // act
            var response = await id.Post($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var roleKey = "one";
            var roleName = "First Role";
            await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            // act
            var response = await id.Post($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var roleKey = "one";
            var roleName = "First Role";

            // act
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);

            // assert
            role.Id.IsNotDefault();
            role.AppId.IsEqual(app.Id);
            role.Key.IsEqual(roleKey);
            role.Name.IsEqual(roleName);
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Get($"/apps/{Guid.NewGuid()}/company-roles").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var roleKey = "one";
            var roleName = "First Role";
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);

            // act
            var roles = await id.Get($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).AsAsync<RoleView[]>();

            // assert
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            roles[0].Claims.IsEmpty();
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var u = new RolePayload { Key = "one" };

            // act
            var response = await id.Put($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var(user, tokens) = await LogUserInAsync();
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_RoleBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app1.Id);
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/apps/{app2.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var other = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, "other", "One Role");
            var u = new RolePayload { Key = "other", Name = "FirstRole" };

            // act
            var response = await id.Put($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<CompanyRoleView>();

            // assert
            response.Id.IsEqual(role.Id);
            response.AppId.IsEqual(app.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
            response.Claims.IsEmpty();
        }

        [Fact]
        public async Task AddClaimToRole_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);
            var p = new ClaimValuePayload { Value = "S" };

            // act
            var response = await id.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaimToRole_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(ownerTokens.AccessToken, app.Id);
            var(user, tokens) = await LogUserInAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaimToRole_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_RoleBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app1.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app1.Id);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaimToRole_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app2.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app1.Id);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await id.Post($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaimToRole_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);
            var claimValue = "Some";

            // act
            var response = await AddCompanyClaimToCompanyRoleAsync(tokens.AccessToken, app.Id, role.Id, claim.Id, claimValue);
            var roles = await id.Get($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).AsAsync<CompanyRoleView[]>();

            // assert
            response.Id.IsEqual(claim.Id);
            response.Key.IsEqual(claim.Key);
            response.Name.IsEqual(claim.Name);
            response.Value.IsEqual(claimValue);
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            var c = roles[0].Claims.At(0);
            c.Id.IsEqual(claim.Id);
            c.Key.IsEqual(claim.Key);
            c.Name.IsEqual(claim.Name);
            c.Value.IsEqual(claimValue);
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(ownerTokens.AccessToken, app.Id);
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_RoleBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app1.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app1.Id);

            // act
            var response = await id.Delete($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app2.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app1.Id);

            // act
            var response = await id.Delete($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaimFromRole_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_RoleBelongsOtherApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app1.Id);

            // act
            var response = await id.Delete($"/apps/{app2.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}