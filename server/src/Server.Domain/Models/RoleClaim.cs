using System;

namespace Server.Domain.Models;

public class RoleClaim
{
    public Guid RoleId { get; private init; }
    public Role Role { get; private init; } = default!;
    public Guid ClaimId { get; private init; }
    public Claim Claim { get; private init; } = default!;
    public string Value { get; private init; } = string.Empty;

    public RoleClaim(
        Role role,
        Claim claim,
        string value
    )
    {
        RoleId = role.Id;
        Role = role;
        ClaimId = claim.Id;
        Claim = claim;
        Value = value;
    }

    internal RoleClaim()
    {
    }
}