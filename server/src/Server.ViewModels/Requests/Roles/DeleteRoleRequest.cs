using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public record DeleteRoleRequest : IRequest<DeleteRoleCommand>
{
    public Guid RoleId { get; set; }
}
