using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Me;

public class UpdateMyPasswordCommand : ICommand
{
    public string Password { get; }
    public User User { get; private set; } = null!;

    public UpdateMyPasswordCommand(
        string password
    )
    {
        Password = password;
    }
}