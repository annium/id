using Annium.Extensions.Composition;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Queries.Companies;

namespace Server.Application.Queries.Companies;

internal class ListMyCompaniesQueryComposer : Composer<ListMyCompaniesQuery>
{
    public ListMyCompaniesQueryComposer(
        ITokenAccessor tokenAccessor,
        IUserRepository userRepository
    )
    {
        Field(c => c.User).LoadWith(_ => userRepository.TryGetByIdAsync(tokenAccessor.GetToken().UserId));
    }
}