using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.ViewModels.Login.Responses
{
    public class TokensResponse : IResponse<Tokens>
    {
        public string AccessToken { get; }
        public Guid RefreshToken { get; }
        public Instant RefreshTokenExpires { get; }

        public TokensResponse(
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