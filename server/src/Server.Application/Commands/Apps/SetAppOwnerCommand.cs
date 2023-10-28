using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Apps;

namespace Server.Application.Commands.Apps;

internal class SetAppOwnerCommandValidator : Validator<SetAppOwnerCommand>
{
    public SetAppOwnerCommandValidator()
    {
        Field(c => c.AppId).Required();
        Field(c => c.NewOwnerId).Required();
    }
}

internal class SetAppOwnerCommandComposer : Composer<SetAppOwnerCommand>
{
    public SetAppOwnerCommandComposer(
        ITokenAccessor tokenAccessor,
        IAppRepository appRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
        Field(c => c.NewOwner).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.NewOwnerId));
    }
}
