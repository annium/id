using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Companies;

public class ListMyCompaniesQuery : IQuery
{
    public User User { get; private set; } = default!;
}