using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Claims;

public class DeleteClaimCommand : ICommand
{
    public Guid ClaimId { get; }
    public Guid MyId { get; private set; }
    public Claim Claim { get; private set; } = null!;

    public DeleteClaimCommand(
        Guid claimId
    )
    {
        ClaimId = claimId;
    }
}