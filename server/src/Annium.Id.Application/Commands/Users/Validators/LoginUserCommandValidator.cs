using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Localization.Abstractions;

namespace Annium.Id.Application.Commands.Users.Validators
{
    internal class LoginUserCommandValidator : Validator<LoginUserCommand>
    {
        public LoginUserCommandValidator(
            IUserRepository userRepository,
            ILocalizer<LoginUserCommandValidator> localizer
        ) : base(localizer)
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
        }
    }
}