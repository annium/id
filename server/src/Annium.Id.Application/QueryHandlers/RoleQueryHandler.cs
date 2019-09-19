using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Application.Queries.Roles;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.QueryHandlers
{
    internal class RoleQueryHandler : IQueryHandler<ListRolesQuery, IEnumerable<Role>>
    {
        private readonly IRoleRepository roleRepository;

        public RoleQueryHandler(
            IRoleRepository roleRepository
        )
        {
            this.roleRepository = roleRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<Role>>> HandleAsync(
            ListRolesQuery request,
            CancellationToken cancellationToken
        )
        {
            var roles = await roleRepository.GetAllAsync(request.AppId);

            return Result.Status(OperationStatus.OK, roles.AsEnumerable());
        }
    }
}