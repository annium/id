using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Application.Queries.Users;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.QueryHandlers
{
    internal class UserQueryHandler : IQueryHandler<GetUserProfileQuery, User>
    {
        public Task<IStatusResult<OperationStatus, User>> HandleAsync(
            GetUserProfileQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Result.New(OperationStatus.OK, request.User));
        }
    }
}