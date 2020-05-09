using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Roles;

namespace Annium.Id.Api.ViewModels.Roles.Requests
{
    public class UpdateRoleRequest : UpdateRoleRequestBase, IRequest<UpdateRoleCommand>
    {
        public Guid RoleId { get; set; }
    }

    public class UpdateRoleRequestBase
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}