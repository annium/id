using Annium.Architecture.CQRS.Queries;
using Core.Domain.Entities;

namespace Server.Domain.Queries.Companies;

public class ListMyCompaniesQuery : IQuery
{
    public User User { get; private set; } = default!;
}