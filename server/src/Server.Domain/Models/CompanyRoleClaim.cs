using System;

namespace Server.Domain.Models;

public class CompanyRoleClaim
{
    public Guid RoleId { get; private init; }
    public CompanyRole Role { get; private init; } = default!;
    public Guid ClaimId { get; private init; }
    public CompanyClaim Claim { get; private init; } = default!;
    public string Value { get; private init; } = string.Empty;

    public CompanyRoleClaim(
        CompanyRole role,
        CompanyClaim claim,
        string value
    )
    {
        RoleId = role.Id;
        Role = role;
        ClaimId = claim.Id;
        Claim = claim;
        Value = value;
    }

    internal CompanyRoleClaim()
    {
    }
}