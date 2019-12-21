using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Apps
{
    public class CreateAppCommand : ICommand
    {
        public string Name { get; }
        public Guid MyId { get; private set; }

        public CreateAppCommand(
            string name
        )
        {
            Name = name;
        }
    }

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
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
        }
    }
}