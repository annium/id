using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

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