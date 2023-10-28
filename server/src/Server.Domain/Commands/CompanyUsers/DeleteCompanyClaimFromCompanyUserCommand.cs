using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyUsers;

public class DeleteCompanyClaimFromCompanyUserCommand : ICommand
{
    public Guid CompanyId { get; }
    public Guid UserId { get; }
    public Guid ClaimId { get; }
    public Guid MyId { get; private set; }
    public Company Company { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public CompanyClaim Claim { get; private set; } = null!;

    public DeleteCompanyClaimFromCompanyUserCommand(Guid companyId, Guid userId, Guid claimId)
    {
        CompanyId = companyId;
        UserId = userId;
        ClaimId = claimId;
    }
}
