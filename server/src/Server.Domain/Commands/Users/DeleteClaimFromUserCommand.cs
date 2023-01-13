using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Users;

public class DeleteClaimFromUserCommand : ICommand
{
    public Guid UserId { get; }
    public Guid ClaimId { get; }
    public Guid MyId { get; private set; }
    public User User { get; private set; } = null!;
    public Claim Claim { get; private set; } = null!;

    public DeleteClaimFromUserCommand(
        Guid userId,
        Guid claimId
    )
    {
        UserId = userId;
        ClaimId = claimId;
    }
}