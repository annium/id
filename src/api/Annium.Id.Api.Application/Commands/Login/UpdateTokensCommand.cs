using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Login;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Login
{
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
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}