using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.AppUsers
{
    public class DeleteRoleFromUserCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public User User { get; private set; }
        public Role Role { get; private set; }

        public DeleteRoleFromUserCommand(
            Guid appId,
            Guid userId,
            Guid roleId
        )
        {
            AppId = appId;
            UserId = userId;
            RoleId = roleId;
        }
    }

    internal class DeleteRoleFromUserCommandValidator : Validator<DeleteRoleFromUserCommand>
    {
        public DeleteRoleFromUserCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.UserId).NotEqual(Guid.Empty);
            Field(c => c.RoleId).NotEqual(Guid.Empty);
        }
    }

    internal class DeleteRoleFromUserCommandComposer : Composer<DeleteRoleFromUserCommand>
    {
        public DeleteRoleFromUserCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}