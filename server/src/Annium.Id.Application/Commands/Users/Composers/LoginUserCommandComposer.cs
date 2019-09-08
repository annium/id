using Annium.Extensions.Composition;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Commands.Users.Composers
{
    internal class LoginUserCommandComposer : Composer<LoginUserCommand>
    {
        public LoginUserCommandComposer(
            IUserRepository userRepository
        )
        {
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}