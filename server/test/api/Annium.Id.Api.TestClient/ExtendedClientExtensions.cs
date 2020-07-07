using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.Id.Api.ViewModels.Login.Requests;
using Annium.Id.Api.ViewModels.Me.Requests;
using Annium.Id.Api.ViewModels.Me.Responses;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Email.Models;
using Annium.Net.Http;

namespace Annium.Id.Api.TestClient
{
    public static class ExtendedClientExtensions
    {
        private static Task<string> RegisterUser(
            this ExtendedClient client,
            string login = "demo",
            string email = "demo@demo.com",
            Guid? referralId = null
        ) => client.RegisterUserInternal(Constants.IdAppId, login, email, referralId);

        private static Task<string> RegisterOtherUser(
            this ExtendedClient client,
            string login = "demo2",
            string email = "demo2@demo.com",
            Guid? referralId = null
        ) => client.RegisterUserInternal(Constants.IdAppId, login, email, referralId);

        public static Task<string> LogUserIn(
            this ExtendedClient client,
            string login = "demo",
            string email = "demo@demo.com",
            string password = "test1test",
            Guid? referralId = null
        ) => client.LogUserInInternal(Constants.IdAppId, login, email, password, referralId);

        public static Task<string> LogOtherUserIn(
            this ExtendedClient client,
            string login = "demo2",
            string email = "demo2@demo.com",
            string password = "test2test",
            Guid? referralId = null
        ) => client.LogUserInInternal(Constants.IdAppId, login, email, password, referralId);

        public static Task<MeResponse> GetUser(
            this ExtendedClient client,
            string login = "demo",
            string email = "demo@demo.com",
            string password = "test1test",
            Guid? referralId = null
        ) => client.GetUserInternal(Constants.IdAppId, login, email, password, referralId);

        public static Task<MeResponse> GetOtherUser(
            this ExtendedClient client,
            string login = "demo2",
            string email = "demo2@demo.com",
            string password = "test2test",
            Guid? referralId = null
        ) => client.GetUserInternal(Constants.IdAppId, login, email, password, referralId);

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
                ReferralId = referralId,
            });

            // get id from email data
            var userId = ((ConfirmEmailData) client.EmailService.Emails.Last().Data).Id;

            // confirm email
            var tokens = await client.Me.ConfirmMyEmail(appId, new ConfirmMyEmailRequestBody { Id = userId });

            return tokens.Data.Data.AccessToken;
        }

        private static async Task<string> LogUserInInternal(
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

        private static async Task<MeResponse> GetUserInternal(
            this ExtendedClient client,
            Guid appId,
            string login = "demo",
            string email = "demo@demo.com",
            string password = "test1test",
            Guid? referralId = null
        )
        {
            var token = await client.LogUserInInternal(appId, login, email, password, referralId);

            var me = await client.WithToken(token).Me.GetMe();

            return me.Data.Data;
        }

        private static ExtendedClient WithToken(
            this ExtendedClient client,
            string accessToken
        ) => client.Request.BearerAuthorization(accessToken).ApiClient(client.EmailService);
    }
}