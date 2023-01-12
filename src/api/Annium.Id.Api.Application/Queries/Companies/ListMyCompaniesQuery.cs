using Annium.Extensions.Composition;
using Annium.Id.Api.Domain.Queries.Companies;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Companies;

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