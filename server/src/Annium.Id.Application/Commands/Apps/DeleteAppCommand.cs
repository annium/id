using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Apps
{
    public class DeleteAppCommand : ICommand
    {
        public Guid AppId { get; set; }
        public Guid MyId { get; private set; }
        public App App { get; private set; }

        public DeleteAppCommand(
            Guid appId
        )
        {
            AppId = appId;
        }
    }

    internal class DeleteAppCommandValidator : Validator<DeleteAppCommand>
    {
        public DeleteAppCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
        }
    }

    internal class DeleteAppCommandComposer : Composer<DeleteAppCommand>
    {
        public DeleteAppCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}