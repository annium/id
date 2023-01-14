using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Apps;

public class DeleteAppCommand : ICommand
{
    public Guid AppId { get; set; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;

    public DeleteAppCommand(
        Guid appId
    )
    {
        AppId = appId;
    }
}