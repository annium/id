using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Validation;
using Annium.Extensions.Composition;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Users
{
    public class GetUserQuery : IQuery
    {
        public Guid UserId { get; }
        public User User { get; private set; } = default!;

        public GetUserQuery(
            Guid userId
        )
        {
            UserId = userId;
        }
    }

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