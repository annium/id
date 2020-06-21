using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Queries.Claims;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.QueryHandlers
{
    public class ClaimQueryHandler :
        IQueryHandler<ListClaimsQuery, IEnumerable<Claim>>
    {
        private readonly IClaimRepository claimRepository;

        public ClaimQueryHandler(
            IClaimRepository claimRepository
        )
        {
            this.claimRepository = claimRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<Claim>>> HandleAsync(
            ListClaimsQuery request,
            CancellationToken cancellationToken
        )
        {
            var claims = await claimRepository.GetAllAsync(request.App.Id);

            return Result.Status(OperationStatus.OK, claims.AsEnumerable());
        }
    }
}