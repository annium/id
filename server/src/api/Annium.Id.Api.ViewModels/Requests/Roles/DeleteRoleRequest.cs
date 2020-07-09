using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Roles;

namespace Annium.Id.Api.ViewModels.Requests.Roles
{
    public class DeleteRoleRequest : IRequest<DeleteRoleCommand>
    {
        public Guid RoleId { get; set; }
    }
}