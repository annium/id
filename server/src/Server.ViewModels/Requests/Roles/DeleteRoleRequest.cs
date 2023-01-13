using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Roles;

namespace Server.ViewModels.Requests.Roles;

public class DeleteRoleRequest : IRequest<DeleteRoleCommand>
{
    public Guid RoleId { get; set; }
}