using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Me;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Me;

internal class ConfirmMyEmailCommandValidator : Validator<ConfirmMyEmailCommand>
{
    public ConfirmMyEmailCommandValidator(
    )
    {
        Field(c => c.AppId).Required();
        Field(c => c.Id).Required();
    }
}

internal class ConfirmMyEmailCommandComposer : Composer<ConfirmMyEmailCommand>
{
    public ConfirmMyEmailCommandComposer(
        IAppRepository appRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.Id));
    }
}