using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.ViewModels.Roles.Requests;
using Annium.Id.ViewModels.Roles.Responses;
using Annium.Net.Http;
using Annium.Testing;
using Xunit;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class RoleControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateRoleRequest() { Key = "one" };

            // act
            var response = await id.Post("/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateRoleRequest() { AppId = Guid.NewGuid(), Key = "one", Name = "First Role" };

            // act
            var response = await id.Post("/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateRoleRequest() { AppId = app.Id, Key = "one", Name = "First Role" };

            // act
            var response = await id.Post("/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NonUniqueKey_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var roleKey = "one";
            var roleName = "First Role";
            await CreateRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);
            var p = new CreateRoleRequest() { AppId = app.Id, Key = roleKey, Name = roleName };

            // act
            var response = await id.Post("/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var roleKey = "one";
            var roleName = "First Role";

            // act
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);

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
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Get("/roles").BearerAuthorization(tokens.AccessToken).Param("appId", Guid.NewGuid()).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task List_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var roleKey = "one";
            var roleName = "First Role";
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);

            // act
            var roles = (await id.Get("/roles").BearerAuthorization(tokens.AccessToken).Param("appId", app.Id).AsResultAsync<RoleResponse[]>()).Data;

            // assert
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            roles[0].Claims.IsEmpty();
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var u = new UpdateRoleRequest { Key = "one" };

            // act
            var response = await id.Put($"/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateRoleRequest { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            await CreateRoleAsync(tokens.AccessToken, app.Id, "other");
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var u = new UpdateRoleRequest { Key = "other", Name = "FirstRole" };

            // act
            var response = await id.Put($"/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateRoleAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateRoleRequest { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var u = new UpdateRoleRequest { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddClaimToRole_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var p = new AddClaimToRoleRequest { Value = "S" };

            // act
            var response = await id.Post($"/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaimToRole_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new AddClaimToRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_MissingClaim_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var p = new AddClaimToRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateRoleAsync(ownerTokens.AccessToken, app.Id);
            var claim = await CreateClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();
            var p = new AddClaimToRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaimToRole_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "Other App");
            var role = await CreateRoleAsync(tokens.AccessToken, app2.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app1.Id);
            var p = new AddClaimToRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaimToRole_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);
            var claimValue = "Some";

            // act
            var response = await AddClaimToRoleAsync(tokens.AccessToken, role.Id, claim.Id, claimValue);
            var roles = (await id.Get("/roles").BearerAuthorization(tokens.AccessToken).Param("appId", app.Id).AsResultAsync<RoleResponse[]>()).Data;

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            var c = roles[0].Claims.At(0);
            c.Id.IsEqual(claim.Id);
            c.Key.IsEqual(claim.Key);
            c.Name.IsEqual(claim.Name);
            c.Value.IsEqual(claimValue);
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingClaim_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateRoleAsync(ownerTokens.AccessToken, app.Id);
            var claim = await CreateClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaimFromRole_ClaimBelongsOtherApp_Forbidden()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app1 = await CreateAppAsync(tokens.AccessToken);
            var app2 = await CreateAppAsync(tokens.AccessToken, "Other App");
            var role = await CreateRoleAsync(tokens.AccessToken, app2.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app1.Id);

            // act
            var response = await id.Delete($"/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaimFromRole_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateClaimAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var role = await CreateRoleAsync(ownerTokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateRoleAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}