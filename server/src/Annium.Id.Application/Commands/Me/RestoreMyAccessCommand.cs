using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Me
{
    public class RestoreMyAccessCommand : ICommand
    {
        public string AppKey { get; set; } = string.Empty;
        public string Server { get; }
        public string Email { get; }
        public App App { get; private set; } = null!;
        public Uri ServerUri { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public RestoreMyAccessCommand(
            string appKey,
            string server,
            string email
        )
        {
            AppKey = appKey;
            Server = server;
            Email = email;
        }
    }

    internal class RestoreMyAccessCommandValidator : Validator<RestoreMyAccessCommand>
    {
        public RestoreMyAccessCommandValidator(
        )
        {
            Field(c => c.AppKey).Required().Length(2, 100);
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
            Field(c => c.App).LoadWith(ctx => appRepository.FindByKeyAsync(ctx.Root.AppKey));
            Field(e => e.ServerUri).LoadWith(ctx => new Uri(ctx.Root.Server));
            Field(c => c.User).LoadWith(ctx => userRepository.FindByEmailAsync(ctx.Root.Email));
        }
    }
}