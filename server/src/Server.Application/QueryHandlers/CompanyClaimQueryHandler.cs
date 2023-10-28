using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Models;
using Server.Domain.Queries.CompanyClaims;

namespace Server.Application.QueryHandlers;

public class CompanyClaimQueryHandler : IQueryHandler<ListCompanyClaimsQuery, IEnumerable<CompanyClaim>>
{
    private readonly ICompanyClaimRepository _companyClaimRepository;

    public CompanyClaimQueryHandler(ICompanyClaimRepository companyClaimRepository)
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
