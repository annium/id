using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Commands.Login;

namespace Server.Application.Commands.Login;

internal class UpdateTokensCommandValidator : Validator<UpdateTokensCommand>
{
    public UpdateTokensCommandValidator()
    {
        Field(e => e.AppId).Required();
        Field(e => e.RefreshToken).Required();
    }
}

internal class UpdateTokensCommandComposer : Composer<UpdateTokensCommand>
{
    public UpdateTokensCommandComposer(
        IAppRepository appRepository,
        IUserLoginRepository userLoginRepository
    )
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
        Field(e => e.Login).LoadWith(ctx => userLoginRepository.TryFindByRefreshTokenAsync(ctx.Root.RefreshToken));
    }
}