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
    public static Task<string> RegisterLogUserIn(
        this ExtendedClient client,
        string? login = null,
        string? email = null,
        string? password = null,
        Guid? referralId = null
    )
    {
        return client.RegisterLogUserInInternal(
            Constants.IdAppId,
            login ?? Faker.Internet.UserName(),
            email ?? Faker.Internet.Email(),
            password ?? Faker.Internet.Password(),
            referralId
        );
    }

    public static async Task<string> LogUserIn(this ExtendedClient client, Guid appId, string login, string password)
    {
        // perform regular login
        var tokens = await client.Login.LogIn(
            appId,
            new LogInRequestBody { Login = login, Password = password },
            Result.New(new TokensResponse()).Error("Failed to load tokens")
        );

        return tokens.Data.Data.AccessToken;
    }

    public static async Task<MeResponse> RegisterLogInGetUser(
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
        var token = await client.RegisterLogUserInInternal(Constants.IdAppId, login, email, password, referralId);

        var me = await client
            .WithToken(token)
            .Me.GetMe(Result.New(new MeResponse()).Error("Failed to load personal info"));

        return me.Data.Data;
    }

    private static async Task<string> RegisterLogUserInInternal(
        this ExtendedClient client,
        Guid appId,
        string login,
        string email,
        string password,
        Guid? referralId
    )
    {
        // register
        var registerResponse = await client.Me.RegisterMe(
            new RegisterMeRequest
            {
                Server = "http://localhost/",
                Login = login,
                Email = email,
                ReferralId = referralId
            },
            Result.New().Error("Failed to register")
        );
        if (registerResponse.StatusCode != HttpStatusCode.OK)
            Console.WriteLine(
                $"Failure: plain errors: {registerResponse.Data.PlainErrors.Join(", ")}; labeled errors: {registerResponse.Data.LabeledErrors.Select(x => $"{x.Key}={x.Value.Join(" + ")}").Join(", ")}"
            );
        registerResponse.StatusCode.Is(HttpStatusCode.OK);

        // get id from email data
        var userId = ((ConfirmEmailData)client.EmailService.Emails.Last().Data).Id;

        // confirm email
        var tokensResponse = await client.Me.ConfirmMyEmail(
            appId,
            new ConfirmMyEmailRequestBody { Id = userId },
            Result.New(new TokensResponse()).Error("Failed to confirm email")
        );
        tokensResponse.StatusCode.Is(HttpStatusCode.OK);

        var token = tokensResponse.Data.Data.AccessToken;

        // set password
        await client
            .WithToken(token)
            .Me.UpdateMyPassword(
                new UpdateMyPasswordRequest { Password = password },
                Result.New().Error("Failed to update password")
            );

        // perform regular login
        var tokens = await client.Login.LogIn(
            appId,
            new LogInRequestBody { Login = login, Password = password },
            Result.New(new TokensResponse()).Error("Failed to log in")
        );

        return tokens.Data.Data.AccessToken;
    }

    private static ExtendedClient WithToken(this ExtendedClient client, string accessToken)
    {
        return client.Request.BearerAuthorization(accessToken).ApiClient(client.EmailService);
    }
}
