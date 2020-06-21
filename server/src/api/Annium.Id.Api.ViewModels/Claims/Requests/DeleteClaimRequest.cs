using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Claims;

namespace Annium.Id.Api.ViewModels.Claims.Requests
{
    public class DeleteClaimRequest : IRequest<DeleteClaimCommand>
    {
        public Guid ClaimId { get; set; }
    }
}