using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Extensions.Net.Http;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Testing;

namespace Annium.Id.IntegrationTests
{
    public class AppControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task Create_InvalidPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new AppPayload() { Key = "de", Name = "Demo App" };

            // act
            var response = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_NonUniqueKey_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var payload = new AppPayload() { Key = "demo", Name = "Demo App" };
            await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

            // act
            var response = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).RunAsync();

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
            var app = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(payload).AsAsync<AppPrivateView>();
            var apps = await id.Get("/apps").AsAsync<AppPrivateView[]>();

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
            var response = await id.Get($"/apps/{Guid.NewGuid()}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetApiToken_NotOwner_Forbidden()
        {
            // arrange
            var(_, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var(_, tokens) = await LoginAsync();

            // act
            var response = await id.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetApiToken_Valid_Ok()
        {
            // arrange
            var(_, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();

            // act
            var response = await id.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsAsync<Guid>();

            // assert
            response.IsNotDefault();
        }

        [Fact]
        public async Task UpdateApiToken_Missing_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateApiToken_NotOwner_Forbidden()
        {
            // arrange
            var(_, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var(_, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateApiToken_Valid_Ok()
        {
            // arrange
            var(_, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var token = await id.Get($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsAsync<Guid>();

            // act
            var response = await id.Post($"/apps/{app.Id}/token").BearerAuthorization(tokens.AccessToken).AsAsync<Guid>();

            // assert
            response.IsNotDefault();
        }

        [Fact]
        public async Task List_Ok()
        {
            // act
            var response = await id.Get("/apps").AsAsync<AppPublicView[]>();

            // assert
            response.IsEmpty();
        }

        [Fact]
        public async Task Update_IncorrectPayload_BadRequest()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var u = new AppPayload { Key = "demo" };

            // act
            var response = await id.Post($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_Missing_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var u = new AppPayload { Key = "demo", Name = "Demo App" };

            // act
            var response = await id.Post($"/apps/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var(user, tokens) = await LoginAsync();
            var u = new AppPayload { Key = "demo", Name = "Demo App" };

            // act
            var response = await id.Post($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_KeyIsNotUnique_Conflict()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p1 = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app1 = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p1).AsAsync<AppPrivateView>();
            var p2 = new AppPayload() { Key = "medo", Name = "Medo App" };
            var app2 = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p2).AsAsync<AppPrivateView>();
            var u = new AppPayload { Key = p1.Key, Name = p2.Name };

            // act
            var response = await id.Post($"/apps/{app2.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Update_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new AppPayload() { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var u = new AppPayload { Key = "medo", Name = "Medo App" };

            // act
            var response = await id.Post($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).JsonContent(u).AsAsync<AppPrivateView>();

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
            var response = await id.Post($"/apps/{Guid.NewGuid()}/owner/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Post($"/apps/{app.Id}/owner/{user.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetOwner_MissingSuccessor_NotFound()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();

            // act
            var response = await id.Post($"/apps/{app.Id}/owner/{Guid.NewGuid()}").BearerAuthorization(ownerTokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SetOwner_Valid_Ok()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var(user, tokens) = await LoginAsync();

            // act
            app = await id.Post($"/apps/{app.Id}/owner/{user.Id}").BearerAuthorization(ownerTokens.AccessToken).AsAsync<AppPrivateView>();

            // assert
            app.OwnerId.IsEqual(user.Id);
        }

        [Fact]
        public async Task Delete_MissingApp_NotFound()
        {
            // arrange
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/apps/{Guid.NewGuid()}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotOwner_Forbidden()
        {
            // arrange
            var(owner, ownerTokens) = await LoginAsync("owner", "superpass", "some@email.com");
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(ownerTokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();
            var(user, tokens) = await LoginAsync();

            // act
            var response = await id.Delete($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Valid_Ok()
        {
            // arrange
            var(user, tokens) = await LoginAsync();
            var p = new AppPayload { Key = "demo", Name = "Demo App" };
            var app = await id.Put("/apps").BearerAuthorization(tokens.AccessToken).JsonContent(p).AsAsync<AppPrivateView>();

            // act
            var response = await id.Delete($"/apps/{app.Id}").BearerAuthorization(tokens.AccessToken).RunAsync();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NoContent);
        }
    }
}