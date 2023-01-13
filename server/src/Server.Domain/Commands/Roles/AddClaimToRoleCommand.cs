using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Roles;

public class AddClaimToRoleCommand : ICommand
{
    public Guid RoleId { get; }
    public Guid ClaimId { get; }
    public string Value { get; }
    public Guid MyId { get; private set; }
    public Role Role { get; private set; } = null!;
    public Claim Claim { get; private set; } = null!;

    public AddClaimToRoleCommand(
        Guid roleId,
        Guid claimId,
        string value
    )
    {
        RoleId = roleId;
        ClaimId = claimId;
        Value = value;
    }
}