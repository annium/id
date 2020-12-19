using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Users
{
    public class AddRoleToUserCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public User User { get; private set; } = null!;
        public Role Role { get; private set; } = null!;

        public AddRoleToUserCommand(
            Guid userId,
            Guid roleId
        )
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}