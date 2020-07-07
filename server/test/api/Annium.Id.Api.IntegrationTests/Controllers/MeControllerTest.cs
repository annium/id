using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annium.Id.Api.TestClient;
using Annium.Id.Api.ViewModels.Me.Requests;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Email.Models;
using Annium.Testing;
using Xunit;

namespace Annium.Id.Api.IntegrationTests.Controllers
{
    public class MeControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task RegisterMe_IncorrectPayload_BadRequest()
        {
            // arrange
            var request = new RegisterMeRequest { Login = "demo" };

            // act
            var response = await Id().Me.RegisterMe(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterMe_LoginIsNotUnique_BadRequest()
        {
            // arrange
            var user = await Id().RegisterLogInGetUser();
            var request = new RegisterMeRequest { Server = "http://localhost/", Login = user.Login, Email = "asd1@demo.com" };

            // act
            var response = await Id().Me.RegisterMe(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterMe_EmailIsNotUnique_BadRequest()
        {
            // arrange
            var user = await Id().RegisterLogInGetUser();
            var request = new RegisterMeRequest { Server = "http://localhost/", Login = "uniqueLogin", Email = user.Email };

            // act
            var response = await Id().Me.RegisterMe(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterMe_ReferralMissing_NotFound()
        {
            // arrange
            var request = new RegisterMeRequest { Server = "http://localhost/", Login = "uniqueLogin", Email = "demo@demo.com", ReferralId = Guid.NewGuid() };

            // act
            var response = await Id().Me.RegisterMe(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RegisterMe_ValidData_Ok()
        {
            // arrange
            var referral = await Id().RegisterLogInGetOtherUser();
            var login = "demo";
            var email = "demo@demo.com";
            var password = "test1test";

            // act
            var response = await Id().RegisterLogInGetUser(login, email, password, referral.Id);

            // assert
            response.Id.IsNotDefault();
            response.Login.IsEqual(login);
            response.Email.IsEqual(email);
            response.ReferralId.IsEqual(referral.Id);
        }

        [Fact]
        public async Task RestoreMyAccess_ValidEmail_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var user = await Id(token).Me.GetMe();
            var request = new RestoreMyAccessRequestBody { Server = "http://localhost/", Email = user.Data.Data.Email };

            // act
            await Id(token).Me.RestoreMyAccess(Constants.IdAppId, request);
            token = emailService.Emails.Last().Data.As<RestoreAccessData>().Tokens.AccessToken;
            var response = await Id(token).Me.GetMe();

            // assert
            response.Data.Data.Id.IsEqual(user.Data.Data.Id);
        }

        [Fact]
        public async Task GetMe_AuthenticatedUser_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Me.GetMe();

            // assert
            response.Data.Data.Id.IsNotDefault();
        }

        [Fact]
        public async Task UpdateMyProfile_IncorrectPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateMyProfileRequest { Login = "demo" };

            // act
            var response = await Id(token).Me.UpdateMyProfile(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateMyProfile_LoginIsNotUnique_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var other = await Id().RegisterLogInGetOtherUser();
            var request = new UpdateMyProfileRequest { Login = other.Login, Email = "asd2@demo.com" };

            // act
            var response = await Id(token).Me.UpdateMyProfile(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateMyProfile_EmailIsNotUnique_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var other = await Id().RegisterLogInGetOtherUser();
            var request = new UpdateMyProfileRequest { Login = "otherLogin", Email = other.Email };

            // act
            var response = await Id(token).Me.UpdateMyProfile(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateMyProfile_ValidData_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateMyProfileRequest { Login = "other", Email = "other@other.com" };

            // act
            var response = await Id(token).Me.UpdateMyProfile(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateMyPassword_IncorrectPayload_BadRequest()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateMyPasswordRequest { Password = "demo" };

            // act
            var response = await Id(token).Me.UpdateMyPassword(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateMyPassword_ValidData_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();
            var request = new UpdateMyPasswordRequest { Password = "MoreSecurePass###" };

            // act
            var response = await Id(token).Me.UpdateMyPassword(request);

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UnregisterMe_ValidData_Ok()
        {
            // arrange
            var token = await Id().RegisterLogUserIn();

            // act
            var response = await Id(token).Me.UnregisterMe();

            // assert
            response.StatusCode.IsEqual(HttpStatusCode.OK);
        }
    }
}