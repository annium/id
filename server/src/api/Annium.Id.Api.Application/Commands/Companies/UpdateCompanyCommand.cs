using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Companies
{
    public class UpdateCompanyCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid? ParentId { get; }
        public string Name { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; } = null!;

        public UpdateCompanyCommand(
            Guid companyId,
            Guid? parentId,
            string name
        )
        {
            CompanyId = companyId;
            ParentId = parentId;
            Name = name;
        }
    }

    internal class UpdateCompanyCommandValidator : Validator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            Field(c => c.CompanyId).Required();
            Field(c => c.ParentId).Required();
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