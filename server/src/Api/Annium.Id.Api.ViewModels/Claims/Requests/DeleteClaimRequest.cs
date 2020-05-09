using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Claims;

namespace Annium.Id.ViewModels.Claims.Requests
{
    public class DeleteClaimRequest : IRequest<DeleteClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }
}