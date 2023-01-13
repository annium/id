using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Core.Domain.Entities;
using Server.Db.Repositories;
using Server.Domain.Queries.Users;

namespace Server.Application.QueryHandlers;

internal class UserQueryHandler :
    IQueryHandler<FindUsersQuery, IEnumerable<User>>,
    IQueryHandler<GetUserQuery, User>
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

        return Result.Status(OperationStatus.Ok, users.AsEnumerable());
    }

    public Task<IStatusResult<OperationStatus, User>> HandleAsync(
        GetUserQuery request,
        CancellationToken ct
    )
    {
        return Task.FromResult(Result.Status(OperationStatus.Ok, request.User));
    }
}