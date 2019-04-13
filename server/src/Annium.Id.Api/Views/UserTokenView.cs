using System;
using Annium.Id.Db;
using Newtonsoft.Json;
using NodaTime;

namespace Annium.Id.Api.Views
{
    public class UserTokenView
    {
        public string AccessToken { get; }

        public Guid RefreshToken { get; }

        public Instant RefreshTokenExpires { get; }

        public UserTokenView(
            string accessToken,
            UserLogin login
        )
        {
            AccessToken = accessToken;
            RefreshToken = login.RefreshToken;
            RefreshTokenExpires = login.RefreshTokenExpires;
        }

        [JsonConstructor]
        public UserTokenView(
            string accessToken,
            Guid refreshToken,
            Instant refreshTokenExpires
        )
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpires = refreshTokenExpires;
        }
    }
}