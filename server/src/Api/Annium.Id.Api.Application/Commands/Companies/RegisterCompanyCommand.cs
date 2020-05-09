using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;

namespace Annium.Id.Api.Application.Commands.Companies
{
    public class RegisterCompanyCommand : ICommand
    {
        public Guid? ParentId { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }

        public RegisterCompanyCommand(
            Guid? parentId,
            string name
        )
        {
            ParentId = parentId;
            Name = name;
        }
    }

    internal class RegisterCompanyCommandValidator : Validator<RegisterCompanyCommand>
    {
        public RegisterCompanyCommandValidator(
        )
        {
            Field(c => c.ParentId).Required();
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class RegisterCompanyCommandComposer : Composer<RegisterCompanyCommand>
    {
        public RegisterCompanyCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
        }
    }
}