using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class LoginUserCommand : ICommand
    {
        public User User { get; private set; }
        public string Login { get; }
        public string Password { get; }

        public LoginUserCommand(
            string login,
            string password
        )
        {
            Login = login;
            Password = password;
        }
    }
}