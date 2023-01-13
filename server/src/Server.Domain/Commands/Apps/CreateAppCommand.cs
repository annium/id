using System;
using Annium.Architecture.CQRS.Commands;

namespace Server.Domain.Commands.Apps;

public class CreateAppCommand : ICommand
{
    public string Name { get; }
    public Guid MyId { get; private set; }

    public CreateAppCommand(
        string name
    )
    {
        Name = name;
    }
}