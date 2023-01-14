using System;

namespace Server.Domain.Models;

public class CompanyRoleClaim
{
    public Guid RoleId { get; }
    public Guid ClaimId { get; }
    public string Value { get; set; }

    public CompanyRoleClaim(
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