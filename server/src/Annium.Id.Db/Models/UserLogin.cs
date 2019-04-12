using System;
using Newtonsoft.Json;
using NodaTime;

namespace Annium.Id.Db
{
    public class UserLogin
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public Instant LoggedAt { get; }

        public string IPAddress { get; }

        public string Client { get; }

        public Guid RefreshToken { get; }

        public Instant RefreshTokenExpires { get; }

        public UserLogin(
            Guid userId,
            Instant loggedAt,
            string ipAddress,
            string client,
            Guid refreshToken,
            Instant refreshTokenExpires
        )
        {
            UserId = userId;
            LoggedAt = loggedAt;
            IPAddress = ipAddress;
            Client = client;
            RefreshToken = refreshToken;
            RefreshTokenExpires = refreshTokenExpires;
        }

        [JsonConstructor]
        internal UserLogin(
            Guid id,
            Guid userId,
            Instant loggedAt,
            string ipAddress,
            string client,
            Guid refreshToken,
            Instant refreshTokenExpires
        ) : this(userId, loggedAt, ipAddress, client, refreshToken, refreshTokenExpires)
        {
            Id = id;
        }

        // TODO: perhaps, separate method for refresh token update
    }
}