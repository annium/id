using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Roles;

namespace Server.Application.Commands.Roles;

internal class CreateRoleCommandValidator : Validator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(IRoleRepository roleRepository)
    {
        Field(c => c.AppId).Required();
        Field(c => c.Key)
            .Required()
            .Length(3, 100)
            .Then()
            .Unique(
                async (c, key) => await roleRepository.TryFindByKeyAsync(c.AppId, key) != null,
                "Role with {1} {2} already exists"
            );
        Field(c => c.Name).Required().Length(3, 100);
    }
}

internal class CreateRoleCommandComposer : Composer<CreateRoleCommand>
{
    public CreateRoleCommandComposer(ITokenAccessor tokenAccessor, IAppRepository appRepository)
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}
