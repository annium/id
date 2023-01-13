using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Commands.Login;

namespace Server.Application.Commands.Login;

internal class LogInCommandValidator : Validator<LogInCommand>
{
    public LogInCommandValidator()
    {
        Field(e => e.AppId).Required();
        Field(e => e.Login).Required().Length(3, 50);
        Field(e => e.Password).Required().Length(8, 50);
    }
}

internal class LogInCommandComposer : Composer<LogInCommand>
{
    public LogInCommandComposer(
        IAppRepository appRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
        Field(c => c.User).LoadWith(ctx => userRepository.TryFindByLoginAsync(ctx.Root.Login));
    }
}