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
        public string Email { get; }
        public App App { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public RestoreMyAccessCommand(
            string appKey,
            string email
        )
        {
            AppKey = appKey;
            Email = email;
        }
    }

    internal class RestoreMyAccessCommandValidator : Validator<RestoreMyAccessCommand>
    {
        public RestoreMyAccessCommandValidator(
        )
        {
            Field(c => c.AppKey).Required();
            Field(c => c.Email).Required();
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
            Field(c => c.User).LoadWith(ctx => userRepository.FindByEmailAsync(ctx.Root.Email));
        }
    }
}