using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Users;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Users;

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
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
        Field(c => c.Role).LoadWith(ctx => roleRepository.TryGetByIdAsync(ctx.Root.RoleId));
    }
}