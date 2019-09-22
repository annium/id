using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Me
{
    public class GetMeQuery : IQuery
    {
        public User User { get; private set; }

        public GetMeQuery()
        {

        }
    }

    internal class GetMeQueryComposer : Composer<GetMeQuery>
    {
        public GetMeQueryComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetBaseToken().UserId));
        }
    }

}