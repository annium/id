using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Application.Queries.Companies;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.QueryHandlers
{
    internal class CompanyQueryHandler :
        IQueryHandler<GetCompanyQuery, Company>,
        IQueryHandler<GetCompanyUsersQuery, IEnumerable<User>>
    {
        private readonly ICompanyUserRepository companyUserRepository;

        public CompanyQueryHandler(
            ICompanyUserRepository companyUserRepository
        )
        {
            this.companyUserRepository = companyUserRepository;
        }

        public Task<IStatusResult<OperationStatus, Company>> HandleAsync(
            GetCompanyQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Result.Status(OperationStatus.OK, request.Company));
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<User>>> HandleAsync(
            GetCompanyUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var company = request.Company;

            var users = await companyUserRepository.GetAllAsync(company.Id);

            return Result.Status(OperationStatus.OK, users.AsEnumerable());
        }
    }
}