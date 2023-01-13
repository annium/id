using System;

namespace Core.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public Guid? ReferralId { get; }

    public User(
        string login,
        string passwordHash,
        string email,
        Guid? referralId
    )
    {
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
        ReferralId = referralId;
    }

    internal User(
        Guid id,
        string login,
        string passwordHash,
        string email,
        Guid? referralId
    ) : this(login, passwordHash, email, referralId)
    {
        Id = id;
    }
}