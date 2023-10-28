using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Me;

namespace Server.Application.Commands.Me;

internal class UpdateMyProfileCommandValidator : Validator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        Field(e => e.Login).Required().Length(3, 50);
        Field(e => e.Email).Required().Length(3, 100).Email();
    }
}

internal class UpdateMyProfileCommandComposer : Composer<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandComposer(ITokenAccessor tokenAccessor, IUserRepository userRepository)
    {
        Field(e => e.User).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}
