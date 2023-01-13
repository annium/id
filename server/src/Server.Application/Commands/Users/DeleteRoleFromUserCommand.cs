using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Users;

namespace Server.Application.Commands.Users;

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
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
        Field(c => c.Role).LoadWith(ctx => roleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}