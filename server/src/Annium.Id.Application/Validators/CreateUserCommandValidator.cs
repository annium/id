using Annium.Extensions.Validation;
using Annium.Id.Application.Commands;
using Annium.Localization.Abstractions;

namespace Annium.Id.Application.Validators
{
    internal class CreateUserCommandValidator : Validator<CreateUserCommand>
    {
        public CreateUserCommandValidator(ILocalizer<CreateUserCommandValidator> localizer) : base(localizer)
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
            Field(e => e.Email).Required().Length(3, 100).Email();
        }
    }
}