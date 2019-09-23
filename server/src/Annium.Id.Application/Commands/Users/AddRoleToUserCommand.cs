using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class AddRoleToUserCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public User User { get; private set; }
        public Role Role { get; private set; }

        public AddRoleToUserCommand(
            Guid userId,
            Guid roleId
        )
        {
            UserId = userId;
            RoleId = roleId;
        }
    }

    internal class AddRoleToUserCommandValidator : Validator<AddRoleToUserCommand>
    {
        public AddRoleToUserCommandValidator()
        {
            Field(c => c.UserId).Required();
            Field(c => c.RoleId).Required();
        }
    }

    internal class AddRoleToUserCommandComposer : Composer<AddRoleToUserCommand>
    {
        public AddRoleToUserCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository,
            IRoleRepository roleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}