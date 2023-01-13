using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Me;

public class UpdateMyProfileCommand : ICommand
{
    public string Login { get; }
    public string Email { get; }
    public User User { get; private set; } = null!;

    public UpdateMyProfileCommand(
        string login,
        string email
    )
    {
        Login = login;
        Email = email;
    }
}