using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Models;
using Server.Domain.Queries.Roles;

namespace Server.Application.QueryHandlers;

internal class RoleQueryHandler : IQueryHandler<ListRolesQuery, IEnumerable<Role>>
{
    private readonly IRoleRepository _roleRepository;

    public RoleQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IStatusResult<OperationStatus, IEnumerable<Role>>> HandleAsync(
        ListRolesQuery request,
        CancellationToken cancellationToken
    )
    {
        var roles = await _roleRepository.GetAllAsync(request.AppId);

        return Result.Status(OperationStatus.Ok, roles.AsEnumerable());
    }
}
