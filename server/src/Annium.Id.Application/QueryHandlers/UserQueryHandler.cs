using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Application.Queries.Users;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.QueryHandlers
{
    internal class UserQueryHandler : IQueryHandler<GetUserQuery, User>
    {
        public Task<IStatusResult<OperationStatus, User>> HandleAsync(
            GetUserQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Result.Status(OperationStatus.OK, request.User));
        }
    }
}