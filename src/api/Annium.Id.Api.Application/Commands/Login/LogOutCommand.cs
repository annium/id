using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Login;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Login;

internal class LogOutCommandValidator : Validator<LogOutCommand>
{
    public LogOutCommandValidator()
    {
        Field(e => e.AppId).Required();
    }
}

internal class LogOutCommandComposer : Composer<LogOutCommand>
{
    public LogOutCommandComposer(
        ITokenAccessor tokenAccessor,
        IAppRepository appRepository
    )
    {
        Field(c => c.LoginId).LoadWith(_ => tokenAccessor.GetToken().LoginId);
        Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
    }
}