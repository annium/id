using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Apps;

public class SetAppOwnerCommand : ICommand
{
    public Guid AppId { get; }
    public Guid NewOwnerId { get; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;
    public User NewOwner { get; private set; } = null!;

    public SetAppOwnerCommand(Guid appId, Guid newOwnerId)
    {
        AppId = appId;
        NewOwnerId = newOwnerId;
    }
}