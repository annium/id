using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyRoles;

public class DeleteCompanyRoleCommand : ICommand
{
    public Guid RoleId { get; }
    public Guid MyId { get; private set; }
    public CompanyRole Role { get; private set; } = null!;

    public DeleteCompanyRoleCommand(
        Guid roleId
    )
    {
        RoleId = roleId;
    }
}