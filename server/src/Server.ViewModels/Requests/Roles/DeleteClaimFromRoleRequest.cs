using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public class DeleteClaimFromRoleRequest : IRequest<DeleteClaimFromRoleCommand>
{
    public Guid RoleId { get; set; }
    public Guid ClaimId { get; set; }
}