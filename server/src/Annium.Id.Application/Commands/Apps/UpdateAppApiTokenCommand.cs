using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Apps
{
    public class UpdateAppApiTokenCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid UserId { get; private set; }
        public App App { get; private set; }

        public UpdateAppApiTokenCommand(
            Guid appId
        )
        {
            AppId = appId;
        }
    }

    internal class UpdateAppApiTokenCommandValidator : Validator<UpdateAppApiTokenCommand>
    {
        public UpdateAppApiTokenCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
        }
    }

    internal class UpdateAppApiTokenCommandComposer : Composer<UpdateAppApiTokenCommand>
    {
        public UpdateAppApiTokenCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.UserId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}