using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Core.Domain.Entities;
using Server.Db.Repositories;
using Server.Domain.Queries.CompanyRoles;

namespace Server.Application.QueryHandlers;

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