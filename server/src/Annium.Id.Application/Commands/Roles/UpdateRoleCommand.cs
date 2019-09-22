using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Roles
{
    public class UpdateRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public Role Role { get; private set; }

        public UpdateRoleCommand(
            Guid roleId,
            string key,
            string name
        )
        {
            RoleId = roleId;
            Key = key;
            Name = name;
        }
    }

    internal class UpdateRoleCommandValidator : Validator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            Field(c => c.RoleId).Required();
            Field(c => c.Key).Required().Length(3, 100);
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateRoleCommandComposer : Composer<UpdateRoleCommand>
    {
        public UpdateRoleCommandComposer(
            ITokenAccessor tokenAccessor,
            IRoleRepository roleRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}