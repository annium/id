using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Companies
{
    public class GetCompanyUsersQuery : IQuery
    {
        public Guid CompanyId { get; }
        public Company Company { get; private set; } = null!;

        public GetCompanyUsersQuery(
            Guid companyId
        )
        {
            CompanyId = companyId;
        }
    }
}