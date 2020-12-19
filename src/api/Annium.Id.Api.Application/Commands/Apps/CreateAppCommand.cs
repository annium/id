using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Apps;
using Annium.Id.Core;

namespace Annium.Id.Api.Application.Commands.Apps
{
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
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        }
    }
}