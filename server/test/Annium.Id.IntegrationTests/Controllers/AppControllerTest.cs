using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Core;
using Annium.Id.ViewModels.Apps.Requests;
using Annium.Id.ViewModels.Apps.Responses;
using Annium.Net.Http;
using Annium.Testing;

namespace Annium.Id.IntegrationTests.Controllers
{
    public class AppControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new CreateAppRequest() { Key = "d", Name = "Demo App" };

            // act
            var response = await id.Post("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_NonUniqueKey_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var payload = new CreateAppRequest() { Key = "demo", Name = "Demo App" };
            await id.Post("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await id.Post("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var appKey = "demo";
            var appName = "Demo App";

            // act
            var app = await CreateAppAsync(tokens.AccessToken, appKey, appName);
            var apps = (await id.Get("/apps").AsResultAsync<AppResponse[]>()).Data;

            // assert
            apps.Has(2);
            app.Key.IsEqual(appKey);
            app.Name.IsEqual(appName);
        }

        [Fact]
        public async Task GetApiToken_Missing_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Get($"/apps/{Guid.NewGuid()}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetApiToken_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetApiToken_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = (await id.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsResultAsync<Guid>()).Data;

            // assert
            response.IsNotDefault();
        }

        [Fact]
        public async Task UpdateApiToken_Missing_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Put($"/apps/{Guid.NewGuid()}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateApiToken_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Put($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateApiToken_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);

            // act
            var response = (await id.Put($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsResultAsync<Guid>()).Data;

            // assert
            response.IsNotDefault();
        }

        [Fact]
        public async Task List_Ok()
        {
            // act
            var response = (await id.Get("/apps").AsResultAsync<AppResponse[]>()).Data;

            // assert
            response.Has(1);
            response.At(0).Key.IsEqual(Constants.IdApp);
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken);
            var u = new UpdateAppRequest { Key = "demo" };

            // act
            var response = await id.Put($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_Missing_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateAppRequest { Key = "demo", Name = "Demo App" };

            // act
            var response = await id.Put($"/apps/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken);
            var tokens = await RegisterLogUserInAsync();
            var u = new UpdateAppRequest { Key = "demo", Name = "Demo App" };

            // act
            var response = await id.Put($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_KeyIsNotUnique_Conflict()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            await CreateAppAsync(tokens.AccessToken, "demo", "Demo App");
            var app2 = await CreateAppAsync(tokens.AccessToken, "medo", "Medo App");
            var u = new UpdateAppRequest { Key = "demo", Name = "Medo App" };

            // act
            var response = await id.Put($"/apps/{app2.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken, "demo", "Demo App");
            var u = new UpdateAppRequest { Key = "medo", Name = "Medo App" };

            // act
            var updateResponse = await id.Put($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();
            updateResponse.StatusCode.IsEqual(HttpStatusCode.OK);
            var response = (await id.Get($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).AsResultAsync<AppResponse>()).Data;

            // assert
            response.Id.IsEqual(app.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
        }

        [Fact]
        public async Task SetOwner_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Put($"/apps/{Guid.NewGuid()}/owner/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken, "demo", "Demo App");
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            var response = await id.Put($"/apps/{app.Id}/owner/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken, "demo", "Demo App");

            // act
            var response = await id.Put($"/apps/{app.Id}/owner/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken, "demo", "Demo App");
            var tokens = await RegisterLogUserInAsync();
            var user = await GetUserAsync(tokens.AccessToken);

            // act
            var response = await id.Put($"/apps/{app.Id}/owner/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            var appTokenResult = await id.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsResultAsync<Guid>();

            // assert
            appTokenResult.HasErrors.IsFalse();
            appTokenResult.Data.IsNotDefault();
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var ownerTokens = await RegisterLogUserInAsync("owner", "superpass", "some@email.com");
            var app = await CreateAppAsync(ownerTokens.AccessToken, "demo", "Demo App");
            var tokens = await RegisterLogUserInAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var tokens = await RegisterLogUserInAsync();
            var app = await CreateAppAsync(tokens.AccessToken, "demo", "Demo App");

            // act
            var response = await id.Delete($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}