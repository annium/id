using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Commands.Users
{
    public class DeleteRoleFromUserCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public User User { get; private set; } = null!;
        public Role Role { get; private set; } = null!;

        public DeleteRoleFromUserCommand(
            Guid userId,
            Guid roleId
        )
        {
            UserId = userId;
            RoleId = roleId;
        }
    }

    internal class DeleteRoleFromUserCommandValidator : Validator<DeleteRoleFromUserCommand>
    {
        public DeleteRoleFromUserCommandValidator()
        {
            Field(c => c.UserId).Required();
            Field(c => c.RoleId).Required();
        }
    }

    internal class DeleteRoleFromUserCommandComposer : Composer<DeleteRoleFromUserCommand>
    {
        public DeleteRoleFromUserCommandComposer(
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