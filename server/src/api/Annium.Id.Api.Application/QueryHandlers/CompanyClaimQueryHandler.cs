using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Queries.CompanyClaims;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.QueryHandlers
{
    public class CompanyClaimQueryHandler :
        IQueryHandler<ListCompanyClaimsQuery, IEnumerable<CompanyClaim>>
    {
        private readonly ICompanyClaimRepository companyClaimRepository;

        public CompanyClaimQueryHandler(
            ICompanyClaimRepository companyClaimRepository
        )
        {
            this.companyClaimRepository = companyClaimRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<CompanyClaim>>> HandleAsync(
            ListCompanyClaimsQuery request,
            CancellationToken cancellationToken
        )
        {
            var claims = await companyClaimRepository.GetAllAsync(request.App.Id);

            return Result.Status(OperationStatus.OK, claims.AsEnumerable());
        }
    }
}