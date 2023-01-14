using System;

namespace Server.Domain.Models;

public class CompanyUserClaim
{
    public Guid CompanyId { get; private init; }
    public Company Company { get; private init; } = default!;
    public Guid UserId { get; private init; }
    public User User { get; private init; } = default!;
    public Guid ClaimId { get; private init; }
    public CompanyClaim Claim { get; private init; } = default!;
    public string Value { get; private init; } = string.Empty;

    public CompanyUserClaim(
        Company company,
        User user,
        CompanyClaim claim,
        string value
    )
    {
        CompanyId = company.Id;
        Company = company;
        UserId = user.Id;
        User = user;
        ClaimId = claim.Id;
        Claim = claim;
        Value = value;
    }

    internal CompanyUserClaim()
    {
    }
}