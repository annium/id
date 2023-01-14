using System;

namespace Server.Domain.Models;

public class UserClaim
{
    public Guid UserId { get; private init; }
    public User User { get; private init; } = default!;
    public Guid ClaimId { get; private init; }
    public Claim Claim { get; private init; } = default!;
    public string Value { get; private init; } = string.Empty;

    public UserClaim(
        User user,
        Claim claim,
        string value
    )
    {
        UserId = user.Id;
        User = user;
        ClaimId = claim.Id;
        Claim = claim;
        Value = value;
    }

    internal UserClaim()
    {
    }
}