using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class LogUserInCommand : ICommand
    {
        public string Login { get; }
        public string Password { get; }
        public User User { get; private set; }

        public LogUserInCommand(
            string login,
            string password
        )
        {
            Login = login;
            Password = password;
        }
    }

    internal class LogUserInCommandValidator : Validator<LogUserInCommand>
    {
        public LogUserInCommandValidator()
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
        }
    }

    internal class LogUserInCommandComposer : Composer<LogUserInCommand>
    {
        public LogUserInCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}