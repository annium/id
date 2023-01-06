using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Apps;

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