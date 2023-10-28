using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyClaims;

namespace Server.ViewModels.Requests.CompanyClaims;

public record DeleteCompanyClaimRequest : IRequest<DeleteCompanyClaimCommand>
{
    public Guid ClaimId { get; set; }
}
