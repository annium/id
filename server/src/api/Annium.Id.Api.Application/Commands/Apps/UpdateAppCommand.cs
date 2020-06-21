using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Apps
{
    public class UpdateAppCommand : ICommand
    {
        public Guid AppId { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; } = null!;

        public UpdateAppCommand(
            Guid appId,
            string name
        )
        {
            AppId = appId;
            Name = name;
        }
    }

    internal class UpdateAppCommandValidator : Validator<UpdateAppCommand>
    {
        public UpdateAppCommandValidator()
        {
            Field(c => c.AppId).Required();
            Field(c => c.Name).Required().Length(2, 100);
        }
    }

    internal class UpdateAppCommandComposer : Composer<UpdateAppCommand>
    {
        public UpdateAppCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}