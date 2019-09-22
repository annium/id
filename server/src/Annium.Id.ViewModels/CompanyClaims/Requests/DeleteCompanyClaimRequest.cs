using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyClaims;

namespace Annium.Id.ViewModels.CompanyClaims.Requests
{
    public class DeleteCompanyClaimRequest : IRequest<DeleteCompanyClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }
}