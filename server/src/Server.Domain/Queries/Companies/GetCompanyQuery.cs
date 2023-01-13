using System;
using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Companies;

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