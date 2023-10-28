using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public record AddClaimToRoleRequest : AddClaimToRoleRequestBody, IRequest<AddClaimToRoleCommand>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
}

public record AddClaimToRoleRequestBody
{
    public string Value { get; set; } = string.Empty;
}
