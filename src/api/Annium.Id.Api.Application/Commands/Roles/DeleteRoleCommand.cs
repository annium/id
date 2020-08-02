using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Roles
{
    public class DeleteRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid MyId { get; private set; }
        public Role Role { get; private set; } = null!;

        public DeleteRoleCommand(
            Guid roleId
        )
        {
            RoleId = roleId;
        }
    }

    internal class DeleteRoleCommandValidator : Validator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            Field(c => c.RoleId).Required();
        }
    }

    internal class DeleteRoleCommandComposer : Composer<DeleteRoleCommand>
    {
        public DeleteRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IRoleRepository roleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}