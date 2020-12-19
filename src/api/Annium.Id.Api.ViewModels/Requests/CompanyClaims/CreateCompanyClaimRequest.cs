using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.CompanyClaims;

namespace Annium.Id.Api.ViewModels.Requests.CompanyClaims
{
    public class CreateCompanyClaimRequest : IRequest<CreateCompanyClaimCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}