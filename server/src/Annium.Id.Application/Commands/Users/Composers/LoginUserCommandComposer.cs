using Annium.Extensions.Composition;
using Annium.Id.Db.Repositories;
using Annium.Localization.Abstractions;

namespace Annium.Id.Application.Commands.Users.Composers
{
    internal class LoginUserCommandComposer : Composer<LoginUserCommand>
    {
        public LoginUserCommandComposer(
            IUserRepository userRepository,
            ILocalizer<LoginUserCommandComposer> localizer
        ) : base(localizer)
        {
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}