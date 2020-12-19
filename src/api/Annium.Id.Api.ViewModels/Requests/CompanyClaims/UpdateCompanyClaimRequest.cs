using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyClaims;

namespace Annium.Id.Api.ViewModels.Requests.CompanyClaims
{
    public class UpdateCompanyClaimRequest : UpdateCompanyClaimRequestBody, IRequest<UpdateCompanyClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }

    public class UpdateCompanyClaimRequestBody
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}