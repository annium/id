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
        public Guid MyId { get; private set; }
        public App App { get; private set; } = null!;

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
            Field(c => c.AppId).Required();
            Field(c => c.Key).Required().Length(2, 100).Then();
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