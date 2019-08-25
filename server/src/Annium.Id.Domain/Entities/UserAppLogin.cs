using System;
using NodaTime;

namespace Annium.Id.Domain.Entities
{
    public class UserAppLogin
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public Guid UserId { get; }
        public Instant LoggedAt { get; }
        public string IPAddress { get; }
        public string Client { get; }
        public Guid RefreshToken { get; set; }
        public Instant RefreshTokenExpires { get; set; }

        public UserAppLogin(
            Guid appId,
            Guid userId,
            Instant loggedAt,
            string ipAddress,
            string client,
            Guid refreshToken,
            Instant refreshTokenExpires
        )
        {
            AppId = appId;
            UserId = userId;
            LoggedAt = loggedAt;
            IPAddress = ipAddress;
            Client = client;
            RefreshToken = refreshToken;
            RefreshTokenExpires = refreshTokenExpires;
        }

        internal UserAppLogin(
            Guid id,
            Guid appId,
            Guid userId,
            Instant loggedAt,
            string ipAddress,
            string client,
            Guid refreshToken,
            Instant refreshTokenExpires
        ) : this(appId, userId, loggedAt, ipAddress, client, refreshToken, refreshTokenExpires)
        {
            Id = id;
        }
    }
}