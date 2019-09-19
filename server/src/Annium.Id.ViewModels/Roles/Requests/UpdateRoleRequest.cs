using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Roles;

namespace Annium.Id.ViewModels.Roles.Requests
{
    public class UpdateRoleRequest : IRequest<UpdateRoleCommand>
    {
        public Guid AppId { get; set; }
        public Guid RoleId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}