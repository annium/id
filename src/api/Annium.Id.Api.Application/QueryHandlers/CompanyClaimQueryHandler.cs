using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Domain.Queries.CompanyClaims;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.QueryHandlers;

public class CompanyClaimQueryHandler :
    IQueryHandler<ListCompanyClaimsQuery, IEnumerable<CompanyClaim>>
{
    private readonly ICompanyClaimRepository _companyClaimRepository;

    public CompanyClaimQueryHandler(
        ICompanyClaimRepository companyClaimRepository
    )
    {
        _companyClaimRepository = companyClaimRepository;
    }

    public async Task<IStatusResult<OperationStatus, IEnumerable<CompanyClaim>>> HandleAsync(
        ListCompanyClaimsQuery request,
        CancellationToken cancellationToken
    )
    {
        var claims = await _companyClaimRepository.GetAllAsync(request.App.Id);

        return Result.Status(OperationStatus.Ok, claims.AsEnumerable());
    }
}