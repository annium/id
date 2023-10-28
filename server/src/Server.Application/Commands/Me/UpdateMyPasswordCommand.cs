using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Me;

namespace Server.Application.Commands.Me;

internal class UpdateMyPasswordCommandValidator : Validator<UpdateMyPasswordCommand>
{
    public UpdateMyPasswordCommandValidator()
    {
        Field(e => e.Password).Required().Length(8, 50);
    }
}

internal class UpdateMyPasswordCommandComposer : Composer<UpdateMyPasswordCommand>
{
    public UpdateMyPasswordCommandComposer(ITokenAccessor tokenAccessor, IUserRepository userRepository)
    {
        Field(e => e.User).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}
