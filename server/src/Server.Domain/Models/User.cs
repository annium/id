using System;
using Annium.Data.Models;

namespace Server.Domain.Models;

public class User : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public string Login { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public Guid? ReferralId { get; private init; }
    public User? Referral { get; private init; }

    public User(
        string login,
        string passwordHash,
        string email,
        User? referral
    )
    {
        Id = Guid.NewGuid();
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
        ReferralId = referral?.Id;
        Referral = referral;
    }

    internal User()
    {
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void Update(string login, string email)
    {
        Login = login;
        Email = email;
    }
}