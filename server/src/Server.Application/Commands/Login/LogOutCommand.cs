using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Login;

namespace Server.Application.Commands.Login;

internal class LogOutCommandValidator : Validator<LogOutCommand>
{
    public LogOutCommandValidator()
    {
        Field(e => e.AppId).Required();
    }
}

internal class LogOutCommandComposer : Composer<LogOutCommand>
{
    public LogOutCommandComposer(ITokenAccessor tokenAccessor, IAppRepository appRepository)
    {
        Field(c => c.LoginId).LoadWith(_ => tokenAccessor.GetToken().LoginId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}
