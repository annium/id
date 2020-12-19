using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Users;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Users
{
    internal class GetUserQueryValidator : Validator<GetUserQuery>
    {
        public GetUserQueryValidator(
        )
        {
            Field(c => c.UserId).Required();
        }
    }

    internal class GetUserQueryComposer : Composer<GetUserQuery>
    {
        public GetUserQueryComposer(
            IUserRepository userRepository
        )
        {
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.UserId));
        }
    }
}