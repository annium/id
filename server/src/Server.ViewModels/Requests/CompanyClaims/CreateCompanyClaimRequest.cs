using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.CompanyClaims;

namespace Server.ViewModels.Requests.CompanyClaims;

public record CreateCompanyClaimRequest : IRequest<CreateCompanyClaimCommand>
{
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}