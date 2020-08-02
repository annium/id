using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Queries.CompanyRoles;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.QueryHandlers
{
    internal class CompanyRoleQueryHandler :
        IQueryHandler<ListCompanyRolesQuery, IEnumerable<CompanyRole>>
    {
        private readonly ICompanyRoleRepository companyRoleRepository;

        public CompanyRoleQueryHandler(
            ICompanyRoleRepository companyRoleRepository
        )
        {
            this.companyRoleRepository = companyRoleRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<CompanyRole>>> HandleAsync(
            ListCompanyRolesQuery request,
            CancellationToken cancellationToken
        )
        {
            var companyRoles = await companyRoleRepository.GetAllAsync(request.AppId);

            return Result.Status(OperationStatus.OK, companyRoles.AsEnumerable());
        }
    }
}