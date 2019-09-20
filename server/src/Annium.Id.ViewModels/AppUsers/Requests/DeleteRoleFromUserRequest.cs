using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.AppUsers;

namespace Annium.Id.ViewModels.AppUsers.Requests
{
    public class DeleteRoleFromUserRequest : IRequest<DeleteRoleFromUserCommand>
    {
        public Guid AppId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}