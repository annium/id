using Annium.Extensions.Validation;
using Annium.Id.Application.Commands;
using Annium.Id.Db.Repositories;
using Annium.Localization.Abstractions;

namespace Annium.Id.Application.Validators
{
    internal class CreateUserCommandValidator : Validator<CreateUserCommand>
    {
        public CreateUserCommandValidator(
            IUserRepository userRepository,
            ILocalizer<CreateUserCommandValidator> localizer
        ) : base(localizer)
        {
            Field(e => e.Login).Required().Length(3, 50).Then()
                .Unique(async login => await userRepository.FindByLoginAsync(login), "User with {1} {2} alrady exists");
            Field(e => e.Password).Required().Length(8, 50);
            Field(e => e.Email).Required().Length(3, 100).Email().Then()
                .Unique(async email => await userRepository.FindByEmailAsync(email), "User with {1} {2} alrady exists");
        }
    }
}