using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Commands.Apps;

namespace Server.Application.Commands.Apps;

internal class CreateAppCommandValidator : Validator<CreateAppCommand>
{
    public CreateAppCommandValidator(
    )
    {
        Field(c => c.Name).Required().Length(2, 100);
    }
}

internal class CreateAppCommandComposer : Composer<CreateAppCommand>
{
    public CreateAppCommandComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository
    )
    {
        Field(c => c.Me).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}