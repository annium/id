using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Commands.Me;

namespace Server.Application.Commands.Me;

internal class ConfirmMyEmailCommandValidator : Validator<ConfirmMyEmailCommand>
{
    public ConfirmMyEmailCommandValidator()
    {
        Field(c => c.AppId).Required();
        Field(c => c.Id).Required();
    }
}

internal class ConfirmMyEmailCommandComposer : Composer<ConfirmMyEmailCommand>
{
    public ConfirmMyEmailCommandComposer(IAppRepository appRepository, IUserRepository userRepository)
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.Id));
    }
}
