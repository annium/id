using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Core;
using Annium.Linq;
using Annium.Net.Http;
using Annium.Testing;
using Server.Email.Models;
using Server.ViewModels.Requests.Login;
using Server.ViewModels.Requests.Me;
using Server.ViewModels.Responses.Login;
using Server.ViewModels.Responses.Me;
using static Server.Host.TestClient.Clients.Helper;

namespace Server.Host.TestClient.Clients;

public static class ExtendedClientExtensions
{
    public static Task<string> RegisterLogUserInAsync(
        this ExtendedClient client,
        string? login = null,
        string? email = null,
        string? password = null,
        Guid? referralId = null
    )
    {
        return client.RegisterLogUserInInternalAsync(
            Constants.IdAppId,
            login ?? Faker.Internet.UserName(),
            email ?? Faker.Internet.Email(),
            password ?? Faker.Internet.Password(),
            referralId
        );
    }

    public static async Task<string> LogUserInAsync(
        this ExtendedClient client,
        Guid appId,
        string login,
        string password
    )
    {
        // perform regular login
        var tokens = await client.Login.LogInAsync(
            appId,
            new LogInRequestBody { Login = login, Password = password },
            Result.Create(new TokensResponse()).Error("Failed to load tokens")
        );

        return tokens.Data.Data.AccessToken;
    }

    public static async Task<MeResponse> RegisterLogInGetUserAsync(
        this ExtendedClient client,
        string? login = null,
        string? email = null,
        string? password = null,
        Guid? referralId = null
    )
    {
        login ??= Faker.Random.String2(30, 40);
        email ??= Faker.Internet.Email();
        password ??= Faker.Internet.Password();
        var token = await client.RegisterLogUserInInternalAsync(Constants.IdAppId, login, email, password, referralId);

        var me = await client
            .WithToken(token)
            .Me.GetMeAsync(Result.Create(new MeResponse()).Error("Failed to load personal info"));

        return me.Data.Data;
    }

    private static async Task<string> RegisterLogUserInInternalAsync(
        this ExtendedClient client,
        Guid appId,
        string login,
        string email,
        string password,
        Guid? referralId
    )
    {
        // register
        var registerResponse = await client.Me.RegisterMeAsync(
            new RegisterMeRequest
            {
                Server = "http://localhost/",
                Login = login,
                Email = email,
                ReferralId = referralId,
            },
            Result.Create().Error("Failed to register")
        );
        if (registerResponse.StatusCode != HttpStatusCode.OK)
            Console.WriteLine(
                $"Failure: plain errors: {registerResponse.Data.PlainErrors.Join(", ")}; labeled errors: {registerResponse.Data.LabeledErrors.Select(x => $"{x.Key}={x.Value.Join(" + ")}").Join(", ")}"
            );
        registerResponse.StatusCode.Is(HttpStatusCode.OK);

        // get id from email data
        var userId = ((ConfirmEmailData)client.EmailService.Emails.Last().Data).Id;

        // confirm email
        var tokensResponse = await client.Me.ConfirmMyEmailAsync(
            appId,
            new ConfirmMyEmailRequestBody { Id = userId },
            Result.Create(new TokensResponse()).Error("Failed to confirm email")
        );
        tokensResponse.StatusCode.Is(HttpStatusCode.OK);

        var token = tokensResponse.Data.Data.AccessToken;

        // set password
        await client
            .WithToken(token)
            .Me.UpdateMyPasswordAsync(
                new UpdateMyPasswordRequest { Password = password },
                Result.Create().Error("Failed to update password")
            );

        // perform regular login
        var tokens = await client.Login.LogInAsync(
            appId,
            new LogInRequestBody { Login = login, Password = password },
            Result.Create(new TokensResponse()).Error("Failed to log in")
        );

        return tokens.Data.Data.AccessToken;
    }

    private static ExtendedClient WithToken(this ExtendedClient client, string accessToken)
    {
        return client.Request.BearerAuthorization(accessToken).ApiClient(client.EmailService);
    }
}
