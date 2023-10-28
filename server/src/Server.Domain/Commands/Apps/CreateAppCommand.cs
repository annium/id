using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Apps;

public class CreateAppCommand : ICommand
{
    public string Name { get; }
    public User Me { get; private set; } = default!;

    public CreateAppCommand(string name)
    {
        Name = name;
    }
}
