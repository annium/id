using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.Users;

namespace Server.Application.Queries.Users;

internal class GetUserQueryValidator : Validator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        Field(c => c.UserId).Required();
    }
}

internal class GetUserQueryComposer : Composer<GetUserQuery>
{
    public GetUserQueryComposer(IUserRepository userRepository)
    {
        Field(c => c.User).LoadWith(ctx => userRepository.TryGetByIdAsync(ctx.Root.UserId));
    }
}
