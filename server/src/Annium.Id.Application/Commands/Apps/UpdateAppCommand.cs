using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Apps
{
    public class UpdateAppCommand : ICommand
    {
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid UserId { get; private set; }
        public App App { get; private set; }

        public UpdateAppCommand(
            Guid appId,
            string key,
            string name
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
        }
    }

    internal class UpdateAppCommandValidator : Validator<UpdateAppCommand>
    {
        public UpdateAppCommandValidator()
        {
            Field(c => c.AppId).NotEqual(Guid.Empty);
            Field(c => c.Key).Required().Length(3, 100).Then();
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateAppCommandComposer : Composer<UpdateAppCommand>
    {
        public UpdateAppCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.UserId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}