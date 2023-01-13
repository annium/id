using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public class AddClaimToRoleRequest : AddClaimToRoleRequestBody, IRequest<AddClaimToRoleCommand>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
}

public class AddClaimToRoleRequestBody
{
    public string Value { get; set; } = string.Empty;
}