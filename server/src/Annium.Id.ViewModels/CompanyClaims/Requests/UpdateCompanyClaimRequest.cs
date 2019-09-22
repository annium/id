using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.CompanyClaims;

namespace Annium.Id.ViewModels.CompanyClaims.Requests
{
    public class UpdateCompanyClaimRequest : IRequest<UpdateCompanyClaimCommand>
    {
        public Guid ClaimId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}