using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Companies
{
    public class UpdateCompanyCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid? ParentId { get; }
        public string Key { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; }

        public UpdateCompanyCommand(
            Guid companyId,
            Guid? parentId,
            string key,
            string name
        )
        {
            CompanyId = companyId;
            ParentId = parentId;
            Key = key;
            Name = name;
        }
    }

    internal class UpdateCompanyCommandValidator : Validator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.ParentId).Required();
            Field(c => c.Key).Required().Length(3, 100);
            Field(c => c.Name).Required().Length(3, 100);
        }
    }

    internal class UpdateCompanyCommandComposer : Composer<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRepository companyRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
        }
    }
}