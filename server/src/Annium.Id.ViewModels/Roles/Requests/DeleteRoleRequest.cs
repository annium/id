using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Roles;

namespace Annium.Id.ViewModels.Roles.Requests
{
    public class DeleteRoleRequest : IRequest<DeleteRoleCommand>
    {
        public Guid AppId { get; set; }
        public Guid RoleId { get; set; }
    }
}