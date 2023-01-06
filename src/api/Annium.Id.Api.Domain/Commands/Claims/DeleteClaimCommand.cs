using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Claims;

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