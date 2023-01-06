using System;

namespace Annium.Id.Domain.Entities;

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