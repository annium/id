using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Queries.CompanyRoles;
using Annium.Id.Api.Domain.Queries.CompanyRoles;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.QueryHandlers
{
    internal class CompanyRoleQueryHandler :
        IQueryHandler<ListCompanyRolesQuery, IEnumerable<CompanyRole>>
    {
        private readonly ICompanyRoleRepository _companyRoleRepository;

        public CompanyRoleQueryHandler(
            ICompanyRoleRepository companyRoleRepository
        )
        {
            _companyRoleRepository = companyRoleRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<CompanyRole>>> HandleAsync(
            ListCompanyRolesQuery request,
            CancellationToken cancellationToken
        )
        {
            var companyRoles = await _companyRoleRepository.GetAllAsync(request.AppId);

            return Result.Status(OperationStatus.Ok, companyRoles.AsEnumerable());
        }
    }
}