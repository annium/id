using Annium.Extensions.Composition;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Localization.Abstractions;

namespace Annium.Id.Application.Queries.Users.Composers
{
    internal class GetUserProfileQueryComposer : Composer<GetUserProfileQuery>
    {
        public GetUserProfileQueryComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository,
            ILocalizer<GetUserProfileQueryComposer> localizer
        ) : base(localizer)
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetBaseToken().UserId));
        }
    }
}