using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Id.Api.ViewModels.Requests.Login;
using Annium.Id.Api.ViewModels.Requests.Me;
using Annium.Id.Api.ViewModels.Responses.Me;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Email.Models;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient;

public static class ExtendedClientExtensions
{
    private static Task<string> RegisterUser(
        this ExtendedClient client,
        string login = "demo",
        string email = "demo@demo.com",
        Guid? referralId = null
    )
    {
        return client.RegisterUserInternal(Constants.IdAppId, login, email, referralId);
    }

    private static Task<string> RegisterOtherUser(
        this ExtendedClient client,
        string login = "demo2",
        string email = "demo2@demo.com",
        Guid? referralId = null
    )
    {
        return client.RegisterUserInternal(Constants.IdAppId, login, email, referralId);
    }

    public static Task<string> RegisterLogUserIn(
        this ExtendedClient client,
        Guid appId,
        string login = "demo",
        string email = "demo@demo.com",
        string password = "test1test",
        Guid? referralId = null
    )
    {
        return client.RegisterLogUserInInternal(appId, login, email, password, referralId);
    }

    public static Task<string> RegisterLogOtherUserIn(
        this ExtendedClient client,
        Guid appId,
        string login = "demo2",
        string email = "demo2@demo.com",
        string password = "test2test",
        Guid? referralId = null
    )
    {
        return client.RegisterLogUserInInternal(appId, login, email, password, referralId);
    }

    public static Task<string> RegisterLogUserIn(
        this ExtendedClient client,
        string login = "demo",
        string email = "demo@demo.com",
        string password = "test1test",
        Guid? referralId = null
    )
    {
        return client.RegisterLogUserInInternal(Constants.IdAppId, login, email, password, referralId);
    }

    public static Task<string> RegisterLogOtherUserIn(
        this ExtendedClient client,
        string login = "demo2",
        string email = "demo2@demo.com",
        string password = "test2test",
        Guid? referralId = null
    )
    {
        return client.RegisterLogUserInInternal(Constants.IdAppId, login, email, password, referralId);
    }

    public static Task<string> LogUserIn(
        this ExtendedClient client,
        Guid appId,
        string login = "demo",
        string password = "test1test"
    )
    {
        return client.LogUserInInternal(appId, login, password);
    }

    public static Task<string> LogOtherUserIn(
        this ExtendedClient client,
        Guid appId,
        string login = "demo2",
        string password = "test2test"
    )
    {
        return client.LogUserInInternal(appId, login, password);
    }

    public static Task<string> LogUserIn(
        this ExtendedClient client,
        string login = "demo",
        string password = "test1test"
    )
    {
        return client.LogUserInInternal(Constants.IdAppId, login, password);
    }

    public static Task<string> LogOtherUserIn(
        this ExtendedClient client,
        string login = "demo2",
        string password = "test2test"
    )
    {
        return client.LogUserInInternal(Constants.IdAppId, login, password);
    }

    public static Task<MeResponse> RegisterLogInGetUser(
        this ExtendedClient client,
        string login = "demo",
        string email = "demo@demo.com",
        string password = "test1test",
        Guid? referralId = null
    )
    {
        return client.RegisterLogInGetUserInternal(Constants.IdAppId, login, email, password, referralId);
    }

    public static Task<MeResponse> RegisterLogInGetOtherUser(
        this ExtendedClient client,
        string login = "demo2",
        string email = "demo2@demo.com",
        string password = "test2test",
        Guid? referralId = null
    )
    {
        return client.RegisterLogInGetUserInternal(Constants.IdAppId, login, email, password, referralId);
    }

    private static async Task<string> RegisterUserInternal(
        this ExtendedClient client,
        Guid appId,
        string login = "demo",
        string email = "demo@demo.com",
        Guid? referralId = null
    )
    {
        // register
        await client.Me.RegisterMe(new RegisterMeRequest
        {
            Server = "http://localhost/",
            Login = login,
            Email = email,
            ReferralId = referralId
        });

        // get id from email data
        var userId = ((ConfirmEmailData) client.EmailService.Emails.Last().Data).Id;

        // confirm email
        var tokens = await client.Me.ConfirmMyEmail(appId, new ConfirmMyEmailRequestBody { Id = userId });

        return tokens.Data.Data.AccessToken;
    }

    private static async Task<string> RegisterLogUserInInternal(
        this ExtendedClient client,
        Guid appId,
        string login = "demo",
        string email = "demo@demo.com",
        string password = "test1test",
        Guid? referralId = null
    )
    {
        var token = await client.RegisterUserInternal(appId, login, email, referralId);

        // set password
        await client.WithToken(token).Me.UpdateMyPassword(new UpdateMyPasswordRequest { Password = password });

        // perform regular login
        var tokens = await client.Login.LogIn(appId, new LogInRequestBody { Login = login, Password = password });

        return tokens.Data.Data.AccessToken;
    }

    private static async Task<string> LogUserInInternal(
        this ExtendedClient client,
        Guid appId,
        string login = "demo",
        string password = "test1test"
    )
    {
        // perform regular login
        var tokens = await client.Login.LogIn(appId, new LogInRequestBody { Login = login, Password = password });

        return tokens.Data.Data.AccessToken;
    }

    private static async Task<MeResponse> RegisterLogInGetUserInternal(
        this ExtendedClient client,
        Guid appId,
        string login = "demo",
        string email = "demo@demo.com",
        string password = "test1test",
        Guid? referralId = null
    )
    {
        var token = await client.RegisterLogUserInInternal(appId, login, email, password, referralId);

        var me = await client.WithToken(token).Me.GetMe();

        return me.Data.Data;
    }

    private static ExtendedClient WithToken(
        this ExtendedClient client,
        string accessToken
    )
    {
        return client.Request.BearerAuthorization(accessToken).ApiClient(client.EmailService);
    }
}