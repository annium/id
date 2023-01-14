using System;
using Annium.Architecture.CQRS.Commands;
using Server.Domain.Models;

namespace Server.Domain.Commands.CompanyClaims;

public class UpdateCompanyClaimCommand : ICommand
{
    public Guid ClaimId { get; }
    public string Key { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public CompanyClaim Claim { get; private set; } = null!;

    public UpdateCompanyClaimCommand(
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