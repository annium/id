using Annium.Architecture.CQRS.Queries;
using Server.Domain.Models;

namespace Server.Domain.Queries.Companies;

public class ListMyCompaniesQuery : IQuery
{
    public User User { get; private set; } = default!;
}
