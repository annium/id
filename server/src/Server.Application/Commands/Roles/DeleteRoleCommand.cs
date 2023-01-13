using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Roles;

namespace Server.Application.Commands.Roles;

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
        Field(c => c.Role).LoadWith(ctx => roleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}