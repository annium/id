using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Me;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Me;

internal class UpdateMyPasswordCommandValidator : Validator<UpdateMyPasswordCommand>
{
    public UpdateMyPasswordCommandValidator(
    )
    {
        Field(e => e.Password).Required().Length(8, 50);
    }
}

internal class UpdateMyPasswordCommandComposer : Composer<UpdateMyPasswordCommand>
{
    public UpdateMyPasswordCommandComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository
    )
    {
        Field(e => e.User).LoadWith(_ => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}