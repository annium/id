using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Claims;

public class UpdateClaimCommand : ICommand
{
    public Guid ClaimId { get; }
    public string Key { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public Claim Claim { get; private set; } = null!;

    public UpdateClaimCommand(
        Guid claimId,
        string key,
        string name
    )
    {
        ClaimId = claimId;
        Key = key;
        Name = name;
    }
}