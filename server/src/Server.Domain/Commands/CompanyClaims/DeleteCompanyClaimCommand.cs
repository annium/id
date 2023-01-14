using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyClaims;

public class DeleteCompanyClaimCommand : ICommand
{
    public Guid ClaimId { get; }
    public Guid MyId { get; private set; }
    public CompanyClaim Claim { get; private set; } = null!;

    public DeleteCompanyClaimCommand(
        Guid claimId
    )
    {
        ClaimId = claimId;
    }
}