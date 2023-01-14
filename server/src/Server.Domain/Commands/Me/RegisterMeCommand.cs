using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Me;

public class RegisterMeCommand : ICommand
{
    public string Server { get; }
    public string Email { get; }
    public string Login { get; }
    public Guid? ReferralId { get; }
    public Uri ServerUri { get; private set; } = default!;
    public User? Referral { get; private set; } = default!;

    public RegisterMeCommand(
        string server,
        string email,
        string login,
        Guid? referralId
    )
    {
        Server = server;
        Email = email;
        Login = login;
        ReferralId = referralId;
    }
}