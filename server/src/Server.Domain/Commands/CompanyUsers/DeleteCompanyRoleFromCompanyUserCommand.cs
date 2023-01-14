using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyUsers;

public class DeleteCompanyRoleFromCompanyUserCommand : ICommand
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid RoleId { get; }
    public Guid MyId { get; private set; }
    public Company Company { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public CompanyRole Role { get; private set; } = null!;

    public DeleteCompanyRoleFromCompanyUserCommand(
        Guid companyId,
        Guid userId,
        Guid roleId
    )
    {
        CompanyId = companyId;
        UserId = userId;
        RoleId = roleId;
    }
}