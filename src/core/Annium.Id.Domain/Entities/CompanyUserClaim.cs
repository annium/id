using System;

namespace Annium.Id.Domain.Entities;

public class CompanyUserClaim
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid ClaimId { get; }
    public string Value { get; set; }

    public CompanyUserClaim(
        Guid companyId,
        Guid userId,
        Guid claimId,
        string value
    )
    {
        CompanyId = companyId;
        UserId = userId;
        ClaimId = claimId;
        Value = value;
    }
}