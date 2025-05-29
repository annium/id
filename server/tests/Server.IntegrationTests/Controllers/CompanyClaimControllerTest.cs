using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Testing;
using Server.Host.TestClient.Clients;
using Server.ViewModels.Requests.CompanyClaims;
using Server.ViewModels.Responses.CompanyClaims;
using Xunit;

namespace Server.IntegrationTests.Controllers;

public class CompanyClaimControllerTest : IntegrationTestBase
{
    public CompanyClaimControllerTest(ITestOutputHelper outputHelper)
        : base(outputHelper) { }

    [Fact]
    public async Task Create_InvalidPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateCompanyClaimRequest { AppId = Guid.NewGuid(), Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .CompanyClaim.CreateCompanyClaimAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_AppMissing_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateCompanyClaimRequest
        {
            AppId = Guid.NewGuid(),
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .CompanyClaim.CreateCompanyClaimAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_NonUniqueKey_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var request = new CreateCompanyClaimRequest
        {
            AppId = app.Id,
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        await Id(token)
            .CompanyClaim.CreateCompanyClaimAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create claim"),
                TestContext.Current.CancellationToken
            );

        // act
        var response = await Id(token)
            .CompanyClaim.CreateCompanyClaimAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_NotOwner_Forbidden()
    {
        // arrange
        var otherToken = await Id().RegisterLogUserInAsync();
        var app = await Id(otherToken).App.RegisterAsync();
        var token = await Id().RegisterLogUserInAsync();
        var request = new CreateCompanyClaimRequest
        {
            AppId = app.Id,
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .CompanyClaim.CreateCompanyClaimAsync(
                request,
                Result.New(Guid.Empty).Error("Failed to create claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_ValidPayload_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claimKey = Faker.Random.String2(5);
        var claimName = Faker.Random.String2(10);

        // act
        var claim = await Id(token).CompanyClaim.RegisterAsync(app.Id, claimKey, claimName);

        // assert
        claim.Id.IsNotDefault();
        claim.IsEqual(
            new
            {
                AppId = app.Id,
                Key = claimKey,
                Name = claimName,
            }
        );
    }

    [Fact]
    public async Task List_MissingApp_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .CompanyClaim.ListCompanyClaimsAsync(
                Guid.NewGuid(),
                Result.New(Array.Empty<CompanyClaimResponse>()).Error("Failed to list claims"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task List_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).CompanyClaim.RegisterAsync(app.Id);

        // act
        var claims = await Id(token)
            .CompanyClaim.ListCompanyClaimsAsync(
                app.Id,
                Result.New(Array.Empty<CompanyClaimResponse>()).Error("Failed to list claims"),
                TestContext.Current.CancellationToken
            )
            .GetDataAsync();

        // assert
        claims.Has(1);
        claims.At(0).Id.Is(claim.Id);
    }

    [Fact]
    public async Task Update_IncorrectPayload_BadRequest()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateCompanyClaimRequestBody { Key = Faker.Random.String2(5) };

        // act
        var response = await Id(token)
            .CompanyClaim.UpdateCompanyClaimAsync(
                Guid.NewGuid(),
                request,
                Result.New().Error("Failed to update claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateCompanyClaimRequestBody
        {
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .CompanyClaim.UpdateCompanyClaimAsync(
                Guid.NewGuid(),
                request,
                Result.New().Error("Failed to update claim"),
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
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();
        var request = new UpdateCompanyClaimRequestBody
        {
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .CompanyClaim.UpdateCompanyClaimAsync(
                claim.Id,
                request,
                Result.New().Error("Failed to update claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_NonUniqueKey_Conflict()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).CompanyClaim.RegisterAsync(app.Id);
        var request = new UpdateCompanyClaimRequestBody
        {
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };
        await Id(token).CompanyClaim.RegisterAsync(app.Id, request.Key, request.Name);

        // act
        var response = await Id(token)
            .CompanyClaim.UpdateCompanyClaimAsync(
                claim.Id,
                request,
                Result.New().Error("Failed to update claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_Valid_Ok()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();
        var app = await Id(token).App.RegisterAsync();
        var claim = await Id(token).CompanyClaim.RegisterAsync(app.Id);
        var request = new UpdateCompanyClaimRequestBody
        {
            Key = Faker.Random.String2(5),
            Name = Faker.Random.String2(10),
        };

        // act
        var response = await Id(token)
            .CompanyClaim.UpdateCompanyClaimAsync(
                claim.Id,
                request,
                Result.New().Error("Failed to update claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_MissingClaim_NotFound()
    {
        // arrange
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .CompanyClaim.DeleteCompanyClaimAsync(
                Guid.NewGuid(),
                Result.New().Error("Failed to delete claim"),
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
        var claim = await Id(otherToken).CompanyClaim.RegisterAsync(app.Id);
        var token = await Id().RegisterLogUserInAsync();

        // act
        var response = await Id(token)
            .CompanyClaim.DeleteCompanyClaimAsync(
                claim.Id,
                Result.New().Error("Failed to delete claim"),
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
        var claim = await Id(token).CompanyClaim.RegisterAsync(app.Id);

        // act
        var response = await Id(token)
            .CompanyClaim.DeleteCompanyClaimAsync(
                claim.Id,
                Result.New().Error("Failed to delete claim"),
                TestContext.Current.CancellationToken
            );

        // assert
        response.StatusCode.Is(HttpStatusCode.OK);
    }
}
