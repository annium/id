using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Roles;

public class DeleteRoleCommand : ICommand
{
    public Guid RoleId { get; }
    public Guid MyId { get; private set; }
    public Role Role { get; private set; } = null!;

    public DeleteRoleCommand(
        Guid roleId
    )
    {
        RoleId = roleId;
    }
}