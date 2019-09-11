using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Users
{
    public class GetUserQuery : IQuery
    {
        public User User { get; private set; }

        public GetUserQuery()
        {

        }
    }

    internal class GetUserProfileQueryComposer : Composer<GetUserQuery>
    {
        public GetUserProfileQueryComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetBaseToken().UserId));
        }
    }

}