using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Companies
{
    public class UnregisterCompanyCommand : ICommand
    {
        public Guid CompanyId { get; }
        public Guid MyId { get; private set; }
        public Company Company { get; private set; }

        public UnregisterCompanyCommand(
            Guid companyId
        )
        {
            CompanyId = companyId;
        }
    }

    internal class UnregisterCompanyCommandValidator : Validator<UnregisterCompanyCommand>
    {
        public UnregisterCompanyCommandValidator()
        {
            Field(c => c.CompanyId).Required();
        }
    }

    internal class UnregisterCompanyCommandComposer : Composer<UnregisterCompanyCommand>
    {
        public UnregisterCompanyCommandComposer(
            ITokenAccessor tokenAccessor,
            ICompanyRepository companyRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
        }
    }
}