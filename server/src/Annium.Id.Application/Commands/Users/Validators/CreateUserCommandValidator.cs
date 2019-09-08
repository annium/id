using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Commands.Users.Validators
{
    internal class CreateUserCommandValidator : Validator<CreateUserCommand>
    {
        public CreateUserCommandValidator(
            IUserRepository userRepository
        )
        {
            Field(e => e.Login).Required().Length(3, 50).Then()
                .Unique(async login => await userRepository.FindByLoginAsync(login) != null, "User with {1} {2} alrady exists");
            Field(e => e.Password).Required().Length(8, 50);
            Field(e => e.Email).Required().Length(3, 100).Email().Then()
                .Unique(async email => await userRepository.FindByEmailAsync(email) != null, "User with {1} {2} alrady exists");
        }
    }
}