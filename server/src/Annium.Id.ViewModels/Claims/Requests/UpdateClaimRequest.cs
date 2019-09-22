using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Claims;

namespace Annium.Id.ViewModels.Claims.Requests
{
    public class UpdateClaimRequest : IRequest<UpdateClaimCommand>
    {
        public Guid ClaimId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}