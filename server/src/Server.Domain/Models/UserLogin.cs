using System;
using Annium.Data.Models;
using NodaTime;

namespace Server.Domain.Models;

public class UserLogin : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public Guid AppId { get; private init; }
    public App App { get; private init; } = default!;
    public Guid UserId { get; private init; }
    public User User { get; private init; } = default!;
    public Instant LoggedAt { get; private init; }
    public string IpAddress { get; private init; } = string.Empty;
    public string Client { get; private init; } = string.Empty;
    public Guid RefreshToken { get; private set; }
    public Instant RefreshTokenExpires { get; private set; }

    public UserLogin(
        App app,
        User user,
        Instant loggedAt,
        string ipAddress,
        string client,
        Guid refreshToken,
        Instant refreshTokenExpires
    )
    {
        Id = Guid.NewGuid();
        AppId = app.Id;
        App = app;
        UserId = user.Id;
        User = user;
        LoggedAt = loggedAt;
        IpAddress = ipAddress;
        Client = client;
        RefreshToken = refreshToken;
        RefreshTokenExpires = refreshTokenExpires;
    }

    internal UserLogin()
    {
    }

    public void Update(Guid refreshToken, Instant refreshTokenExpires)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpires = refreshTokenExpires;
    }
}