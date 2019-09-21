using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Roles
{
    public class DeleteRoleCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public Role Role { get; private set; }

        public DeleteRoleCommand(
            Guid appId,
            Guid roleId
        )
        {
            AppId = appId;
            RoleId = roleId;
        }
    }

    internal class DeleteRoleCommandValidator : Validator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.RoleId).NotEqual(Guid.Empty);
        }
    }

    internal class DeleteRoleCommandComposer : Composer<DeleteRoleCommand>
    {
        public DeleteRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IRoleRepository roleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}