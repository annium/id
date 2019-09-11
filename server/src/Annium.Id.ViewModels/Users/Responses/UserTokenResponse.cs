using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.ViewModels.Users.Responses
{
    public class UserTokenResponse : IResponse<UserToken>
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