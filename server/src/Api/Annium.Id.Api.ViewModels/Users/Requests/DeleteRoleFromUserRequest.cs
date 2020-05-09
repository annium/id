using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class DeleteRoleFromUserRequest : IRequest<DeleteRoleFromUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}