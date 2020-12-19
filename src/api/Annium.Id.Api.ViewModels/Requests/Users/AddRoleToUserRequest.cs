using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Users;

namespace Annium.Id.Api.ViewModels.Requests.Users
{
    public class AddRoleToUserRequest : IRequest<AddRoleToUserCommand>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}