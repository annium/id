using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Commands.Apps
{
    public class CreateAppCommand : ICommand
    {
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }

        public CreateAppCommand(
            string key,
            string name
        )
        {
            Key = key;
            Name = name;
        }
    }

    internal class CreateAppCommandValidator : Validator<CreateAppCommand>
    {
        public CreateAppCommandValidator(
            IAppRepository appRepository
        )
        {
            Field(c => c.Key).Required().Length(3, 100).Then()
                .Unique(async(c, key) => await appRepository.FindByKeyAsync(key) != null, "App with {1} {2} already exists");
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class CreateAppCommandComposer : Composer<CreateAppCommand>
    {
        public CreateAppCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
        }
    }
}