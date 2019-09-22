using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Login
{
    public class LogInCommand : ICommand
    {
        public string Login { get; }
        public string Password { get; }
        public User User { get; private set; }

        public LogInCommand(
            string login,
            string password
        )
        {
            Login = login;
            Password = password;
        }
    }

    internal class LogInCommandValidator : Validator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
        }
    }

    internal class LogInCommandComposer : Composer<LogInCommand>
    {
        public LogInCommandComposer(
            IUserRepository userRepository
        )
        {
            Field(c => c.User).LoadWith(ctx => userRepository.FindByLoginAsync(ctx.Root.Login));
        }
    }
}