using System;
using NodaTime;

namespace Annium.Id.ViewModels.User.Responses
{
    public class UserTokenResponse
    {
        public string AccessToken { get; }
        public Guid RefreshToken { get; }
        public Instant RefreshTokenExpires { get; }

        public UserTokenResponse(
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