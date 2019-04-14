using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.Api.IntegrationTests
{
    [Skip]
    public class AppControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new AppPayload() { Key = "de", Name = "Demo App" };

            // act
            var response = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new AppPayload() { Key = "demo", Name = "Demo App" };
            await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_ValidPayload_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new AppPayload() { Key = "demo", Name = "Demo App" };

            // act
            var app = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).AsAsync<AppView>();
            var apps = await http.Get("/apps").AsAsync<AppView[]>();

            // assert
            apps.Has(1);
            app.Key.IsEqual(payload.Key);
            app.Name.IsEqual(payload.Name);
        }

        [Fact]
        public async Task GetApiToken_Missing_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Get($"/apps/{Guid.NewGuid()}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetApiToken_NotOwner_Forbidden()
        {
            // arrange
            var(_, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var(_, tokens) = await LoginAsync();

            // act
            var response = await http.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetApiToken_Valid_Ok()
        {
            // arrange
            var(_, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppView>();

            // act
            var response = await http.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsAsync<Guid>();

            // assert
            response.IsNotDefault();
        }

        [Fact]
        public async Task UpdateApiToken_Missing_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateApiToken_NotOwner_Forbidden()
        {
            // arrange
            var(_, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var(_, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateApiToken_Valid_Ok()
        {
            // arrange
            var(_, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var token = await http.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsAsync<Guid>();

            // act
            var response = await http.Post($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsAsync<Guid>();

            // assert
            response.IsNotDefault();
        }

        [Fact]
        public async Task List_Ok()
        {
            // act
            var response = await http.Get("/apps").RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
            (await response.Content.ReadAsStringAsync()).IsEqual("[]");
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var u = new AppPayload { Key = "demo" };

            // act
            var response = await http.Post($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_Missing_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var u = new AppPayload { Key = "demo", Name = "Demo App" };

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var(user, tokens) = await LoginAsync();
            var u = new AppPayload { Key = "demo", Name = "Demo App" };

            // act
            var response = await http.Post($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_KeyIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p1 = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app1 = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p1).AsAsync<AppView>();
            var p2 = new AppPayload() { Key = "medo", Name = "Medo App" };
            var app2 = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p2).AsAsync<AppView>();
            var u = new AppPayload { Key = p1.Key, Name = p2.Name };

            // act
            var response = await http.Post($"/apps/{app2.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var u = new AppPayload { Key = "medo", Name = "Medo App" };

            // act
            var response = await http.Post($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<AppView>();

            // assert
            response.Id.IsEqual(app.Id);
            response.Key.IsEqual(u.Key);
            response.Name.IsEqual(u.Name);
        }

        [Fact]
        public async Task SetOwner_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/apps/{Guid.NewGuid()}/owner/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Post($"/apps/{app.Id}/owner/{user.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();

            // act
            var response = await http.Post($"/apps/{app.Id}/owner/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var(user, tokens) = await LoginAsync();

            // act
            app = await http.Post($"/apps/{app.Id}/owner/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();

            // assert
            app.OwnerId.IsEqual(user.Id);
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppView>();
            var(user, tokens) = await LoginAsync();

            // act
            var response = await http.Delete($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await http.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppView>();

            // act
            var response = await http.Delete($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}