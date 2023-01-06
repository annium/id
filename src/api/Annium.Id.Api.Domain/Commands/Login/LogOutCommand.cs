using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Login;

public class LogOutCommand : ICommand
{
    public Guid AppId { get; }
    public Guid LoginId { get; private set; }
    public App App { get; private set; } = null!;

    public LogOutCommand(
        Guid appId
    )
    {
        AppId = appId;
    }
}