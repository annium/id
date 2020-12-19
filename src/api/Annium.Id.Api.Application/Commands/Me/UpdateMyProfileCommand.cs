using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Me;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Me
{
    internal class UpdateMyProfileCommandValidator : Validator<UpdateMyProfileCommand>
    {
        public UpdateMyProfileCommandValidator(
        )
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Email).Required().Length(3, 100).Email();
        }
    }

    internal class UpdateMyProfileCommandComposer : Composer<UpdateMyProfileCommand>
    {
        public UpdateMyProfileCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
        }
    }
}