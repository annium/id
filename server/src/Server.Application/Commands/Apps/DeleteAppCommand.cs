using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Apps;

namespace Server.Application.Commands.Apps;

internal class DeleteAppCommandValidator : Validator<DeleteAppCommand>
{
    public DeleteAppCommandValidator()
    {
        Field(c => c.AppId).Required();
    }
}

internal class DeleteAppCommandComposer : Composer<DeleteAppCommand>
{
    public DeleteAppCommandComposer(
        ITokenAccessor tokenAccessor,
        IAppRepository appRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}