using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Commands.Users
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

    internal class CreateUserCommandValidator : Validator<CreateUserCommand>
    {
        public CreateUserCommandValidator(
            IUserRepository userRepository
        )
        {
            Field(e => e.Login).Required().Length(3, 50).Then()
                .Unique(async(c, login) => await userRepository.FindByLoginAsync(login) != null, "User with {1} {2} alrady exists");
            Field(e => e.Password).Required().Length(8, 50);
            Field(e => e.Email).Required().Length(3, 100).Email().Then()
                .Unique(async(c, email) => await userRepository.FindByEmailAsync(email) != null, "User with {1} {2} alrady exists");
        }
    }
}