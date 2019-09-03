using System;
using NodaTime;

namespace Annium.Id.Domain.Entities
{
    public class UserToken
    {
        public string AccessToken { get; }
        public Guid RefreshToken { get; }
        public Instant RefreshTokenExpires { get; }

        public UserToken(
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