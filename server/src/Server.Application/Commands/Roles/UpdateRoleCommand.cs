using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Roles;

namespace Server.Application.Commands.Roles;

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
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.Role).LoadWith(ctx => roleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}