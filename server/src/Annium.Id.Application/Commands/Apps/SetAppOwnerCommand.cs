using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Apps
{
    public class SetAppOwnerCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid NewOwnerId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }
        public User NewOwner { get; private set; }

        public SetAppOwnerCommand(Guid appId, Guid newOwnerId)
        {
            AppId = appId;
            NewOwnerId = newOwnerId;
        }
    }

    internal class SetAppOwnerCommandValidator : Validator<SetAppOwnerCommand>
    {
        public SetAppOwnerCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.NewOwnerId).NotEqual(Guid.Empty);
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
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.NewOwner).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.NewOwnerId));
        }
    }
}