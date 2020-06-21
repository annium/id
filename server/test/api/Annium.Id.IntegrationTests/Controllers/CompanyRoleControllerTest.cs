using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.ViewModels.CompanyClaims.Responses;
using Annium.Id.Api.ViewModels.CompanyRoles.Requests;
using Annium.Id.Api.ViewModels.CompanyRoles.Responses;
using Annium.Net.Http;
using Annium.Testing;
using Xunit;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class CompanyRoleControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateCompanyRoleRequest() { AppId = Guid.NewGuid(), Key = "one" };

            // act
            var response = await id.Post($"/companies/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_AppMissing_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new CreateCompanyRoleRequest() { AppId = Guid.NewGuid(), Key = "one", Name = "First Role" };

            // act
            var response = await id.Post($"/companies/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

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
            var p = new CreateCompanyRoleRequest() { AppId = app.Id, Key = "one", Name = "First Role" };

            // act
            var response = await id.Post($"/companies/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

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
            await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);
            var p = new CreateCompanyRoleRequest() { AppId = app.Id, Key = "one", Name = "First Role" };

            // act
            var response = await id.Post($"/companies/roles").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

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
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);

            // assert
            role.Id.IsNotDefault();
            role.IsEqual(new { AppId = app.Id, Key = roleKey, Name = roleName });
        }

        [Fact]
        public async Task List_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Get($"/companies/roles").BearerAuthorization(tokens.AccessToken).Param("appId", Guid.NewGuid()).RunAsync();

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
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, roleKey, roleName);

            // act
            var roles =
                (await id.Get($"/companies/roles").BearerAuthorization(tokens.AccessToken).Param("appId", app.Id).AsResultAsync<CompanyRoleResponse[]>()).Data;

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
            var u = new UpdateCompanyRoleRequest { Key = "one" };

            // act
            var response = await id.Put($"/companies/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyRoleRequest { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/companies/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateCompanyRoleRequest { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/companies/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_NonUniqueKey_Conflict()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            await CreateCompanyRoleAsync(tokens.AccessToken, app.Id, "other", "One Role");
            var u = new UpdateCompanyRoleRequest { Key = "other", Name = "FirstRole" };

            // act
            var response = await id.Put($"/companies/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var u = new UpdateCompanyRoleRequest { Key = "one", Name = "One Role" };

            // act
            var response = await id.Put($"/companies/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddClaimToRole_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new AddCompanyClaimToCompanyRoleRequest { Value = "S" };

            // act
            var response = await id.Post($"/companies/roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p)
                .RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddClaimToRole_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var p = new AddCompanyClaimToCompanyRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/companies/roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p)
                .RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_MissingClaim_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var p = new AddCompanyClaimToCompanyRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/companies/roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(p)
                .RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddClaimToRole_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();
            var p = new AddCompanyClaimToCompanyRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/companies/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

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
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app2.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app1.Id);
            var p = new AddCompanyClaimToCompanyRoleRequest { Value = "Some" };

            // act
            var response = await id.Post($"/companies/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AddClaimToRole_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);
            var claimValue = "Some";

            // act
            var response = await AddCompanyClaimToCompanyRoleAsync(tokens.AccessToken, role.Id, claim.Id, claimValue);
            var roles =
                (await id.Get($"/companies/roles").BearerAuthorization(tokens.AccessToken).Param("appId", app.Id).AsResultAsync<CompanyRoleResponse[]>()).Data;

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            roles.Has(1);
            roles[0].Id.IsEqual(role.Id);
            var c = roles[0].Claims.At(0);
            c.IsEqual(new CompanyClaimValueResponse { Id = claim.Id, Key = claim.Key, Name = claim.Name, Value = claimValue });
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/roles/{Guid.NewGuid()}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_MissingClaim_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/companies/roles/{role.Id}/claims/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteClaimFromRole_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

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
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app2.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app1.Id);

            // act
            var response = await id.Delete($"/companies/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteClaimFromRole_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);
            var claim = await CreateCompanyClaimAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/companies/roles/{role.Id}/claims/{claim.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_MissingRole_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/roles/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var role = await CreateCompanyRoleAsync(ownerTokens.AccessToken, app.Id);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/companies/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var role = await CreateCompanyRoleAsync(tokens.AccessToken, app.Id);

            // act
            var response = await id.Delete($"/companies/roles/{role.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}