using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Apps
{
    public class ListMyAppsQuery : IQuery
    {
        public User User { get; private set; } = default!;

        public ListMyAppsQuery(
        )
        {
        }
    }


    internal class ListMyAppsQueryComposer : Composer<ListMyAppsQuery>
    {
        public ListMyAppsQueryComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
        }
    }
}