using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Models.Extensions;
using Annium.Id.Core;
using Annium.Testing;
using Server.TestClient;
using Server.ViewModels.Requests.Apps;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class AppControllerTest : IntegrationTestBase
{
    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateAppRequest { Name = Faker.Random.String2(1, 1) };

        // act
        var response = await Id(token).App.CreateApp(request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var appName = Faker.Random.String2(10);

        // act
        var app = await Id(token).App.Register(appName);
        var apps = await Id(token).App.FindApps(appName).GetData();

        // assert
        apps.Has(1);
        apps.ElementAt(0).Is(app);
        app.Name.Is(appName);
    }

    [Fact]
    public async Task GetApiToken_Missing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.GetAppApiToken(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetApiToken_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.GetAppApiToken(app.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetApiToken_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.GetAppApiToken(app.Id).GetData();

        // assert
        response.IsNotDefault();
    }

    [Fact]
    public async Task UpdateApiToken_Missing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.UpdateAppApiToken(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateApiToken_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.UpdateAppApiToken(app.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateApiToken_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.UpdateAppApiToken(app.Id);

        // assert
        response.Data.Data.IsNotDefault();
    }

    [Fact]
    public async Task Find_All_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.FindApps(string.Empty).GetData();

        // assert
        response.Has(1);
        response.At(0).Id.Is(Constants.IdAppId);
    }

    [Fact]
    public async Task Find_Query_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.FindApps(app.Name).GetData();

        // assert
        response.IsShallowEqual(new[] { app });
    }

    [Fact]
    public async Task ListMy_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.ListMyApps().GetData();

        // assert
        response.IsShallowEqual(new[] { app });
    }

    [Fact]
    public async Task Get_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.GetApp(app.Id).GetData();

        // assert
        response.IsShallowEqual(app);
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(1, 1) };

        // act
        var response = await Id(token).App.UpdateApp(app.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Missing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).App.UpdateApp(Guid.NewGuid(), request);

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).App.UpdateApp(app.Id, request);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).App.UpdateApp(app.Id, request);
        var result = await Id(token).App.GetApp(app.Id).GetData();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        result.Id.Is(app.Id);
        result.Name.Is(request.Name);
    }

    [Fact]
    public async Task SetOwner_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.SetAppOwner(Guid.NewGuid(), Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var me = await Id(token).Me.GetMe().GetData();

        // act
        var response = await Id(token).App.SetAppOwner(app.Id, me.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SetOwner_MissingSuccessor_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.SetAppOwner(app.Id, Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var otherToken = await Id().RegisterLogUserIn();
        var user = await Id(otherToken).Me.GetMe().GetData();

        // act
        var response = await Id(token).App.SetAppOwner(app.Id, user.Id);
        var appTokenResult = await Id(otherToken).App.GetAppApiToken(app.Id).GetResult();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        appTokenResult.HasErrors.IsFalse();
        appTokenResult.Data.IsNotDefault();
    }

    [Fact]
    public async Task Delete_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.DeleteApp(Guid.NewGuid());

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).App.DeleteApp(app.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();

        // act
        var response = await Id(token).App.DeleteApp(app.Id);

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}