using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Queries.Users;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.QueryHandlers
{
    internal class UserQueryHandler :
        IQueryHandler<FindUsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _userRepository;

        public UserQueryHandler(
            IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<User>>> HandleAsync(
            FindUsersQuery request,
            CancellationToken ct
        )
        {
            var users = await _userRepository.FindAllByQueryAsync(request.Query, request.Limit);

            return Result.Status(OperationStatus.OK, users.AsEnumerable());
        }
    }
}