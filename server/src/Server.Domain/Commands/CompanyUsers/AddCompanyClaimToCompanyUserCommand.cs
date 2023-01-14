using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyUsers;

public class AddCompanyClaimToCompanyUserCommand : ICommand
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid ClaimId { get; }
    public string Value { get; }
    public Guid MyId { get; private set; }
    public Company Company { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public CompanyClaim Claim { get; private set; } = null!;

    public AddCompanyClaimToCompanyUserCommand(
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