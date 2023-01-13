using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Claims;

namespace Server.ViewModels.Requests.Claims;

public class DeleteClaimRequest : IRequest<DeleteClaimCommand>
{
    public Guid ClaimId { get; set; }
}