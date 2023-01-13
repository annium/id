using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.CompanyRoles;

public class UpdateCompanyRoleCommand : ICommand
{
    public Guid RoleId { get; }
    public string Key { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public CompanyRole Role { get; private set; } = null!;

    public UpdateCompanyRoleCommand(
        Guid roleId,
        string key,
        string name
    )
    {
        RoleId = roleId;
        Key = key;
        Name = name;
    }
}