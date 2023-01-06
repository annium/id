using System;
using Annium.Architecture.CQRS.Commands;

namespace Annium.Id.Api.Domain.Commands.Apps;

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