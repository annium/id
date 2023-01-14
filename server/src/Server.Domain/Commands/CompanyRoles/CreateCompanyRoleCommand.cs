using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyRoles;

public class CreateCompanyRoleCommand : ICommand
{
    public Guid AppId { get; }
    public string Key { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;

    public CreateCompanyRoleCommand(
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