using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Roles;

public class CreateRoleCommand : ICommand
{
    public Guid AppId { get; }
    public string Key { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;

    public CreateRoleCommand(
        Guid appId,
        string key,
        string name
    )
    {
        AppId = appId;
        Key = key;
        Name = name;
    }
}