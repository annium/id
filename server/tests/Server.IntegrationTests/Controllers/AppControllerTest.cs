using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Models.Extensions;
using Annium.Data.Operations;
using Annium.Id.Core;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Apps;
using Server.ViewModels.Responses.Apps;
using Server.ViewModels.Responses.Me;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class AppControllerTest : IntegrationTestBase
{
    public AppControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateAppRequest { Name = Faker.Random.String2(1, 1) };

        // act
        var response = await Id(token)
            .App.CreateAppAsync(
                request,
                Result.Create(Guid.Empty).Error("Failed to create app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var appName = Faker.Random.String2(10);

        // act
        var app = await Id(token).App.RegisterAsync(appName);
        var apps = await Id(token)
            .App.FindAppsAsync(
                appName,
                Result.Create(Array.Empty<AppResponse>()).Error("Failed to find apps"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        apps.Has(1);
        apps.ElementAt(0).Is(app);
        app.Name.Is(appName);
    }

    [Fact]
    public async Task GetApiToken_Missing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.GetAppApiTokenAsync(
                Guid.NewGuid(),
                Result.Create(Guid.Empty).Error("Failed to get app API token"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetApiToken_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.GetAppApiTokenAsync(
                app.Id,
                Result.Create(Guid.Empty).Error("Failed to get app API token"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetApiToken_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.GetAppApiTokenAsync(
                app.Id,
                Result.Create(Guid.Empty).Error("Failed to get app API token"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsNotDefault();
    }

    [Fact]
    public async Task UpdateApiToken_Missing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.UpdateAppApiTokenAsync(
                Guid.NewGuid(),
                Result.Create(Guid.Empty).Error("Failed to update app API token"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateApiToken_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.UpdateAppApiTokenAsync(
                app.Id,
                Result.Create(Guid.Empty).Error("Failed to update app API token"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateApiToken_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.UpdateAppApiTokenAsync(
                app.Id,
                Result.Create(Guid.Empty).Error("Failed to update app API token"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.Data.Data.IsNotDefault();
    }

    [Fact]
    public async Task Find_All_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.FindAppsAsync(
                string.Empty,
                Result.Create(Array.Empty<AppResponse>()).Error("Failed to find apps"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsNotEmpty();
        response.Any(x => x.Id == Constants.IdAppId).IsTrue();
    }

    [Fact]
    public async Task Find_Query_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.FindAppsAsync(
                app.Name,
                Result.Create(Array.Empty<AppResponse>()).Error("Failed to find apps"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(new[] { app });
    }

    [Fact]
    public async Task ListMy_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.ListMyAppsAsync(
                Result.Create(Array.Empty<AppResponse>()).Error("Failed to list my apps"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(new[] { app });
    }

    [Fact]
    public async Task Get_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.GetAppAsync(
                app.Id,
                Result.Create(new AppResponse()).Error("Failed to get app"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.IsShallowEqual(app);
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(1, 1) };

        // act
        var response = await Id(token)
            .App.UpdateAppAsync(
                app.Id,
                request,
                Result.Create().Error("Failed to update app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Missing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .App.UpdateAppAsync(
                Guid.NewGuid(),
                request,
                Result.Create().Error("Failed to update app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .App.UpdateAppAsync(
                app.Id,
                request,
                Result.Create().Error("Failed to update app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var request = new UpdateAppRequestBody { Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token)
            .App.UpdateAppAsync(
                app.Id,
                request,
                Result.Create().Error("Failed to update app"),
                TestContext.Current.CancellationToken
            );
        var result = await Id(token)
            .App.GetAppAsync(
                app.Id,
                Result.Create(new AppResponse()).Error("Failed to get app"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        result.Id.Is(app.Id);
        result.Name.Is(request.Name);
    }

    [Fact]
    public async Task SetOwner_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.SetAppOwnerAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Result.Create().Error("Failed to set app owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var me = await Id(token)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token)
            .App.SetAppOwnerAsync(
                app.Id,
                me.Id,
                Result.Create().Error("Failed to set app owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SetOwner_MissingSuccessor_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.SetAppOwnerAsync(
                app.Id,
                Guid.NewGuid(),
                Result.Create().Error("Failed to set app owner"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetOwner_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var otherToken = await Id().RegisterLogUserInAsync();
        var user = await Id(otherToken)
            .Me.GetMeAsync(
                Result.Create(new MeResponse()).Error("Failed to load personal information"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // act
        var response = await Id(token)
            .App.SetAppOwnerAsync(
                app.Id,
                user.Id,
                Result.Create().Error("Failed to set app owner"),
                TestContext.Current.CancellationToken
            );
        var appTokenResult = await Id(otherToken)
            .App.GetAppApiTokenAsync(
                app.Id,
                Result.Create(Guid.Empty).Error("Failed to get app API token"),
                TestContext.Current.CancellationToken
            )
            .GetResultAsync();

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
        appTokenResult.HasErrors.IsFalse();
        appTokenResult.Data.IsNotDefault();
    }

    [Fact]
    public async Task Delete_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.DeleteAppAsync(
                Guid.NewGuid(),
                Result.Create().Error("Failed to delete app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .App.DeleteAppAsync(
                app.Id,
                Result.Create().Error("Failed to delete app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();

        // act
        var response = await Id(token)
            .App.DeleteAppAsync(
                app.Id,
                Result.Create().Error("Failed to delete app"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
