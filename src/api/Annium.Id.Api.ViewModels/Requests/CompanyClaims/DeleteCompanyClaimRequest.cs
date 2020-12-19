using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyClaims;

namespace Annium.Id.Api.ViewModels.Requests.CompanyClaims
{
    public class DeleteCompanyClaimRequest : IRequest<DeleteCompanyClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }
}