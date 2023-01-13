using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Apps;

public class UpdateAppCommand : ICommand
{
    public Guid AppId { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;

    public UpdateAppCommand(
        Guid appId,
        string name
    )
    {
        AppId = appId;
        Name = name;
    }
}