using Annium.Architecture.CQRS.Commands;

namespace Annium.Id.Application.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Login { get; }
        public string Password { get; }
        public string Email { get; }

        public CreateUserCommand(
            string login,
            string password,
            string email
        )
        {
            Login = login;
            Password = password;
            Email = email;
        }
    }
}