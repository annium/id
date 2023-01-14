using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.Users;

public class AddClaimToUserCommand : ICommand
{
    public Guid UserId { get; }
    public Guid ClaimId { get; }
    public string Value { get; }
    public Guid MyId { get; private set; }
    public User User { get; private set; } = null!;
    public Claim Claim { get; private set; } = null!;

    public AddClaimToUserCommand(
        Guid userId,
        Guid claimId,
        string value
    )
    {
        UserId = userId;
        ClaimId = claimId;
        Value = value;
    }
}