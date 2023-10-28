using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyClaims;

namespace Server.ViewModels.Requests.CompanyClaims;

public record UpdateCompanyClaimRequest : UpdateCompanyClaimRequestBody, IRequest<UpdateCompanyClaimCommand>
{
    public Guid ClaimId { get; set; }
}

public record UpdateCompanyClaimRequestBody
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
