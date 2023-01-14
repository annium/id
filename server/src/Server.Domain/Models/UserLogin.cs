using System;
using NodaTime;

namespace Server.Domain.Models;

public class UserLogin
{
    public Guid Id { get; }
    public Guid AppId { get; }
    public Guid UserId { get; }
    public Instant LoggedAt { get; }
    public string IpAddress { get; }
    public string Client { get; }
    public Guid RefreshToken { get; set; }
    public Instant RefreshTokenExpires { get; set; }

    public UserLogin(
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
        IpAddress = ipAddress;
        Client = client;
        RefreshToken = refreshToken;
        RefreshTokenExpires = refreshTokenExpires;
    }

    internal UserLogin(
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