using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Companies
{
    public class GetCompanyUsersQuery : IQuery
    {
        public Guid CompanyId { get; }
        public Company Company { get; private set; }

        public GetCompanyUsersQuery(
            Guid companyId
        )
        {
            CompanyId = companyId;
        }
    }

    internal class GetCompanyUsersQueryValidator : Validator<GetCompanyUsersQuery>
    {
        public GetCompanyUsersQueryValidator()
        {
            Field(c => c.CompanyId).Required();
        }
    }

    internal class GetCompanyUsersQueryComposer : Composer<GetCompanyUsersQuery>
    {
        public GetCompanyUsersQueryComposer(
            ICompanyRepository companyRepository
        )
        {
            Field(c => c.Company).LoadWith(ctx => companyRepository.GetByIdAsync(ctx.Root.CompanyId));
        }
    }
}