using System;
using NodaTime;

namespace Annium.Id.Domain.Entities
{
    public class Tokens
    {
        public string AccessToken { get; }
        public Guid RefreshToken { get; }
        public Instant RefreshTokenExpires { get; }

        public Tokens(
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