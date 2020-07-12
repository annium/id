using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Me
{
    public class GetMyTokenQuery : IQuery
    {
        public UserLogin Login { get; private set; }

        public GetMyTokenQuery(
        )
        {
        }
    }

    internal class GetTokenQueryValidator : Validator<GetMyTokenQuery>
    {
        public GetTokenQueryValidator(
        )
        {
        }
    }

    internal class GetTokenQueryComposer : Composer<GetMyTokenQuery>
    {
        public GetTokenQueryComposer(
            ITokenAccessor tokenAccessor,
            IUserLoginRepository userLoginRepository
        )
        {
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.GetByIdAsync(tokenAccessor.GetToken().LoginId));
        }
    }
}