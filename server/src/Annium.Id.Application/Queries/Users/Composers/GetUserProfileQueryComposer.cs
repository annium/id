using Annium.Extensions.Composition;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;

namespace Annium.Id.Application.Queries.Users.Composers
{
    internal class GetUserProfileQueryComposer : Composer<GetUserProfileQuery>
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