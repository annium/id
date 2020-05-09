using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Users;

namespace Annium.Id.Api.ViewModels.Users.Requests
{
    public class DeleteRoleFromUserRequest : IRequest<DeleteRoleFromUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}