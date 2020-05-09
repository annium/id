using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Roles;

namespace Annium.Id.Api.ViewModels.Roles.Requests
{
    public class DeleteRoleRequest : IRequest<DeleteRoleCommand>
    {
        public Guid RoleId { get; set; }
    }
}