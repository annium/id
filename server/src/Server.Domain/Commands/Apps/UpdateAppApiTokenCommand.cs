using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Apps;

public class UpdateAppApiTokenCommand : ICommand
{
    public Guid AppId { get; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;

    public UpdateAppApiTokenCommand(
        Guid appId
    )
    {
        AppId = appId;
    }
}