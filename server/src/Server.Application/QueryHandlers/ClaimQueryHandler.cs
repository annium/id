using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Models;
using Server.Domain.Queries.Claims;

namespace Server.Application.QueryHandlers;

public class ClaimQueryHandler :
    IQueryHandler<ListClaimsQuery, IEnumerable<Claim>>
{
    private readonly IClaimRepository _claimRepository;

    public ClaimQueryHandler(
        IClaimRepository claimRepository
    )
    {
        _claimRepository = claimRepository;
    }

    public async Task<IStatusResult<OperationStatus, IEnumerable<Claim>>> HandleAsync(
        ListClaimsQuery request,
        CancellationToken cancellationToken
    )
    {
        var claims = await _claimRepository.GetAllAsync(request.App.Id);

        return Result.Status(OperationStatus.Ok, claims.AsEnumerable());
    }
}