using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.CompanyClaims;

namespace Annium.Id.Api.ViewModels.CompanyClaims.Requests
{
    public class UpdateCompanyClaimRequest : UpdateCompanyClaimRequestBase, IRequest<UpdateCompanyClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }

    public class UpdateCompanyClaimRequestBase
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}