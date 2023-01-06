using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Login;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Login;

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
        Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
    }
}