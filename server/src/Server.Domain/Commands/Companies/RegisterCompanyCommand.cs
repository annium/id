using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Companies;

public class RegisterCompanyCommand : ICommand
{
    public Guid? ParentId { get; }
    public string Name { get; }
    public Company? Parent { get; private set; } = default;
    public User Me { get; private set; } = default!;

    public RegisterCompanyCommand(Guid? parentId, string name)
    {
        ParentId = parentId;
        Name = name;
    }
}
