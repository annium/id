using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Claims;

namespace Annium.Id.ViewModels.Claims.Requests
{
    public class UpdateClaimRequest : UpdateClaimRequestBase, IRequest<UpdateClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }

    public class UpdateClaimRequestBase
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}