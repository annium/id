using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Roles;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Roles
{
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
            Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
            Field(c => c.Role).LoadWith(ctx => roleRepository.GetByIdAsync(ctx.Root.RoleId));
        }
    }
}