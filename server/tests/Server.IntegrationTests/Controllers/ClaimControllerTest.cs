using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.Claims;
using Server.ViewModels.Responses.Claims;
using Xunit;
using Xunit.Abstractions;

namespace Server.IntegrationTests.Controllers;

public class ClaimControllerTest : IntegrationTestBase
{
    public ClaimControllerTest(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateClaimRequest { AppId = Guid.Empty, Key = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.CreateClaim(request, Result.New(Guid.Empty).Error("Failed to create claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new CreateClaimRequest { AppId = Guid.NewGuid(), Key = Faker.Random.String2(10), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.CreateClaim(request, Result.New(Guid.Empty).Error("Failed to create claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var token = await Id().RegisterLogUserIn();
        var request = new CreateClaimRequest { AppId = app.Id, Key = Faker.Random.String2(10), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.CreateClaim(request, Result.New(Guid.Empty).Error("Failed to create claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_NonUniqueKey_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var request = new CreateClaimRequest { AppId = app.Id, Key = Faker.Random.String2(10), Name = Faker.Random.String2(10) };

        await Id(token).Claim.CreateClaim(request, Result.New(Guid.Empty).Error("Failed to create claim"));

        // act
        var response = await Id(token).Claim.CreateClaim(request, Result.New(Guid.Empty).Error("Failed to create claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claimKey = Faker.Random.String2(5);
        var claimName = Faker.Random.String2(10);

        // act
        var claim = await Id(token).Claim.Register(app.Id, claimKey, claimName);

        // assert
        claim.Id.IsNotDefault();
        claim.AppId.Is(app.Id);
        claim.Key.Is(claimKey);
        claim.Name.Is(claimName);
    }

    [Fact]
    public async Task List_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Claim.ListClaims(Guid.NewGuid(), Result.New(Array.Empty<ClaimResponse>()).Error("Failed to list claims"));

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task List_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);

        // act
        var claims = await Id(token).Claim.ListClaims(app.Id, Result.New(Array.Empty<ClaimResponse>()).Error("Failed to list claims")).GetData();

        // assert
        claims.Has(1);
        claims.At(0).Id.Is(claim.Id);
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);
        var request = new UpdateClaimRequestBody { Key = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.UpdateClaim(claim.Id, request, Result.New().Error("Failed to update claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateClaimRequestBody { Key = Faker.Random.String2(10), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.UpdateClaim(claim.Id, request, Result.New().Error("Failed to update claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var request = new UpdateClaimRequestBody { Key = Faker.Random.String2(10), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.UpdateClaim(Guid.NewGuid(), request, Result.New().Error("Failed to update claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);
        var request = new UpdateClaimRequestBody { Key = Faker.Random.String2(5), Name = Faker.Random.String2(10) };
        await Id(token).Claim.Register(app.Id, request.Key, request.Name);

        // act
        var response = await Id(token).Claim.UpdateClaim(claim.Id, request, Result.New().Error("Failed to update claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);
        var request = new UpdateClaimRequestBody { Key = Faker.Random.String2(10), Name = Faker.Random.String2(10) };

        // act
        var response = await Id(token).Claim.UpdateClaim(claim.Id, request, Result.New().Error("Failed to update claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserIn();
        var app = await Id(otherToken).App.Register();
        var claim = await Id(otherToken).Claim.Register(app.Id);
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Claim.DeleteClaim(claim.Id, Result.New().Error("Failed to delete claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();

        // act
        var response = await Id(token).Claim.DeleteClaim(Guid.NewGuid(), Result.New().Error("Failed to delete claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserIn();
        var app = await Id(token).App.Register();
        var claim = await Id(token).Claim.Register(app.Id);

        // act
        var response = await Id(token).Claim.DeleteClaim(claim.Id, Result.New().Error("Failed to delete claim"));

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}