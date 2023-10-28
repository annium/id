using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;
using Server.ViewModels.Responses.CompanyClaims;

namespace Server.ViewModels.Responses.CompanyRoleClaims;

public record CompanyRoleClaimResponse : IResponse<CompanyRoleClaim>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
    public CompanyClaimResponse Claim { get; set; } = new();
    public string Value { get; set; } = string.Empty;
}
