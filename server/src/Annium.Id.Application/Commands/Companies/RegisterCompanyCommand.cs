using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Commands.Companies
{
    public class RegisterCompanyCommand : ICommand
    {
        public Guid? ParentId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }

        public RegisterCompanyCommand(
            Guid? parentId,
            string key,
            string name
        )
        {
            ParentId = parentId;
            Key = key;
            Name = name;
        }
    }

    internal class RegisterCompanyCommandValidator : Validator<RegisterCompanyCommand>
    {
        public RegisterCompanyCommandValidator(
            ICompanyRepository companyRepository
        )
        {
            Field(c => c.ParentId).Required();
            Field(c => c.Key).Required().Length(3, 100).Then()
                .Unique(async(c, key) => await companyRepository.FindByKeyAsync(key) != null, "Company with {1} {2} already exists");
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