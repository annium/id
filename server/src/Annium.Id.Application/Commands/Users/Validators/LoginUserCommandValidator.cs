using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Commands.Users.Validators
{
    internal class LoginUserCommandValidator : Validator<LoginUserCommand>
    {
        public LoginUserCommandValidator(
            IUserRepository userRepository
        )
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
        }
    }
}