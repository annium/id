using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Users;

namespace Server.ViewModels.Requests.Users;

public record DeleteRoleFromUserRequest : IRequest<DeleteRoleFromUserCommand>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}