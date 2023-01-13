using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyClaims;

namespace Server.ViewModels.Requests.CompanyClaims;

public class DeleteCompanyClaimRequest : IRequest<DeleteCompanyClaimCommand>
{
    public Guid ClaimId { get; set; }
}