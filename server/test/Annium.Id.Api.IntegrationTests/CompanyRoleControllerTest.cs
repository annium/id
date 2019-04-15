using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.Db;
using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    [Skip]
    public class CompanyRoleControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new RolePayload() { Key = "one" };

            // act
            var response = await http.Put($"/apps/{Guid.NewGuid()}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            // act
            var response = await http.Put($"/apps/{Guid.NewGuid()}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            // act
            var response = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // act
            var response = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };

            // act
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();

            // assert
            role.Id.IsNotDefault();
            role.AppId.IsEqual(app.Id);
            role.Key.IsEqual(p.Key);
            role.Name.IsEqual(p.Name);
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Get($"/apps/{Guid.NewGuid()}/company-roles").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();

            // act
            var roles = await http.Get($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).AsAsync<CompanyRoleView[]>();

            // assert
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            roles[0].Claims.IsEmpty();
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var u = new RolePayload { Key = "one" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var(user, tokens) = await LoginAsync();
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_RoleBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app1.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await http.Post($"/apps/{app2.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var u = new RolePayload { Key = "other", Name = "One Role" };
            await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<CompanyRoleView>();

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();
            var u = new RolePayload { Key = "one", Name = "One Role" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<CompanyRoleView>();

            // assert
            response.Id.IsEqual(role.Id);
            response.AppId.IsEqual(app.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
            response.Claims.IsEmpty();
        }

        [Fact]
        public async Task AddClaim_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "S" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaim_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_RoleBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await http.Put($"/apps/{app1.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app1.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaim_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await http.Put($"/apps/{app2.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app1.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaim_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Post($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<ClaimValueView>();
            var roles = await http.Get($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).AsAsync<CompanyRoleView[]>();

            // assert
            response.Id.IsEqual(claim.Id);
            response.Key.IsEqual(claim.Key);
            response.Name.IsEqual(claim.Name);
            response.Value.IsEqual(p.Value);
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            var c = roles[0].Claims.At(0);
            c.Id.IsEqual(claim.Id);
            c.Key.IsEqual(claim.Key);
            c.Name.IsEqual(claim.Name);
            c.Value.IsEqual(p.Value);
        }

        [Fact]
        public async Task DeleteClaim_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(ownerTokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var(user, tokens) = await LoginAsync();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_RoleBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await http.Put($"/apps/{app1.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app1.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_MissingClaim_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaim_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var role = await http.Put($"/apps/{app2.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app1.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{app2.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaim_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new RolePayload() { Key = "one", Name = "First Role" }).AsAsync<CompanyRoleView>();
            var claim = await http.Put($"/apps/{app.Id}/company-claims").BearerAuthorization(tokens.AccessToken)
                .JsonContent(new ClaimPayload() { Key = "asdasd", Name = "First Claim" }).AsAsync<CompanyRoleView>();
            var p = new ClaimValuePayload { Value = "Some" };

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{Guid.NewGuid()}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_MissingRole_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_RoleBelongsOtherApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "other", "Other App");
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app1.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();

            // act
            var response = await http.Delete($"/apps/{app2.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var p = new RolePayload() { Key = "one", Name = "First Role" };
            var role = await http.Put($"/apps/{app.Id}/company-roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<CompanyRoleView>();

            // act
            var response = await http.Delete($"/apps/{app.Id}/company-roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}