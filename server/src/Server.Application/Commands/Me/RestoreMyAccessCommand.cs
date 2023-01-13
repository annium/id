using System;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Commands.Me;

namespace Server.Application.Commands.Me;

internal class RestoreMyAccessCommandValidator : Validator<RestoreMyAccessCommand>
{
    public RestoreMyAccessCommandValidator(
    )
    {
        Field(c => c.AppId).Required();
        Field(e => e.Server).Required().Must(x => Uri.TryCreate(x, UriKind.Absolute, out _), "Server Uri is not valid");
        Field(e => e.Email).Required().Length(3, 100).Email();
    }
}

internal class RestoreMyAccessCommandComposer : Composer<RestoreMyAccessCommand>
{
    public RestoreMyAccessCommandComposer(
        IAppRepository appRepository,
        IUserRepository userRepository
    )
    {
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
        Field(e => e.ServerUri).LoadWith(ctx => new Uri(ctx.Root.Server));
        Field(c => c.User).LoadWith(ctx => userRepository.TryFindByEmailAsync(ctx.Root.Email));
    }
}