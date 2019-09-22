using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class AddRoleToUserRequest : IRequest<AddRoleToUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}