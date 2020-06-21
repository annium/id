using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Queries.Companies
{
    public class GetCompanyQuery : IQuery
    {
        public Guid CompanyId { get; }
        public Company Company { get; private set; } = null!;

        public GetCompanyQuery(
            Guid companyId
        )
        {
            CompanyId = companyId;
        }
    }

    internal class GetCompanyQueryValidator : Validator<GetCompanyQuery>
    {
        public GetCompanyQueryValidator()
        {
            Field(c => c.CompanyId).Required();
        }
    }

    internal class GetCompanyQueryComposer : Composer<GetCompanyQuery>
    {
        public GetCompanyQueryComposer(
            ICompanyRepository companyRepository
        )
        {
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
        }
    }
}