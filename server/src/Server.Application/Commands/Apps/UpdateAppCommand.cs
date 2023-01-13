using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Apps;

namespace Server.Application.Commands.Apps;

internal class UpdateAppCommandValidator : Validator<UpdateAppCommand>
{
    public UpdateAppCommandValidator()
    {
        Field(c => c.AppId).Required();
        Field(c => c.Name).Required().Length(2, 100);
    }
}

internal class UpdateAppCommandComposer : Composer<UpdateAppCommand>
{
    public UpdateAppCommandComposer(
        ITokenAccessor tokenAccessor,
        IAppRepository appRepository
    )
    {
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}