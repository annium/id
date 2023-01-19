using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Claims;

namespace Server.ViewModels.Requests.Claims;

public record UpdateClaimRequest : UpdateClaimRequestBody, IRequest<UpdateClaimCommand>
{
    public Guid ClaimId { get; set; }
}

public record UpdateClaimRequestBody
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}