using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;
using Server.ViewModels.Responses.Claims;

namespace Server.ViewModels.Responses.RoleClaims;

public record RoleClaimResponse : IResponse<RoleClaim>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
    public ClaimResponse Claim { get; set; } = new();
    public string Value { get; set; } = string.Empty;
}
