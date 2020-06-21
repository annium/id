using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyClaims;

namespace Annium.Id.Api.ViewModels.CompanyClaims.Requests
{
    public class DeleteCompanyClaimRequest : IRequest<DeleteCompanyClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }
}