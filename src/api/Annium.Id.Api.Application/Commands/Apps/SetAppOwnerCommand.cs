using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Commands.Apps;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Apps
{
    internal class SetAppOwnerCommandValidator : Validator<SetAppOwnerCommand>
    {
        public SetAppOwnerCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.NewOwnerId).Required();
        }
    }

    internal class SetAppOwnerCommandComposer : Composer<SetAppOwnerCommand>
    {
        public SetAppOwnerCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository,
            IUserRepository userRepository
        )
        {
            Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.NewOwner).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.NewOwnerId));
        }
    }
}