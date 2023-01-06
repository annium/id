using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Claims;

namespace Annium.Id.Api.ViewModels.Requests.Claims;

public class DeleteClaimRequest : IRequest<DeleteClaimCommand>
{
    public Guid ClaimId { get; set; }
}