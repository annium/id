using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Claims;

namespace Annium.Id.Api.ViewModels.Requests.Claims;

public class UpdateClaimRequest : UpdateClaimRequestBody, IRequest<UpdateClaimCommand>
{
    public Guid ClaimId { get; set; }
}

public class UpdateClaimRequestBody
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}