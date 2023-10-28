using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Roles;

public class DeleteClaimFromRoleCommand : ICommand
{
    public Guid RoleId { get; }
    public Guid ClaimId { get; }
    public Guid MyId { get; private set; }
    public Role Role { get; private set; } = null!;
    public Claim Claim { get; private set; } = null!;

    public DeleteClaimFromRoleCommand(Guid roleId, Guid claimId)
    {
        RoleId = roleId;
        ClaimId = claimId;
    }
}
