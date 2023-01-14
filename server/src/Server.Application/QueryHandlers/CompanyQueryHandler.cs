using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Models;
using Server.Domain.Queries.Companies;

namespace Server.Application.QueryHandlers;

internal class CompanyQueryHandler :
    IQueryHandler<FindCompaniesQuery, IEnumerable<Company>>,
    IQueryHandler<ListMyCompaniesQuery, IEnumerable<Company>>,
    IQueryHandler<GetCompanyQuery, Company>,
    IQueryHandler<GetCompanyUsersQuery, IEnumerable<User>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyUserRepository _companyUserRepository;

    public CompanyQueryHandler(
        ICompanyRepository companyRepository,
        ICompanyUserRepository companyUserRepository
    )
    {
        _companyRepository = companyRepository;
        _companyUserRepository = companyUserRepository;
    }

    public async Task<IStatusResult<OperationStatus, IEnumerable<Company>>> HandleAsync(
        FindCompaniesQuery request,
        CancellationToken cancellationToken
    )
    {
        var companies = await _companyRepository.FindAllAsync(request.Query);

        return Result.Status(OperationStatus.Ok, companies.AsEnumerable());
    }

    public async Task<IStatusResult<OperationStatus, IEnumerable<Company>>> HandleAsync(
        ListMyCompaniesQuery request,
        CancellationToken cancellationToken
    )
    {
        var companies = await _companyRepository.FindMyAsync(request.User.Id);

        return Result.Status(OperationStatus.Ok, companies.AsEnumerable());
    }

    public Task<IStatusResult<OperationStatus, Company>> HandleAsync(
        GetCompanyQuery request,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(Result.Status(OperationStatus.Ok, request.Company));
    }

    public async Task<IStatusResult<OperationStatus, IEnumerable<User>>> HandleAsync(
        GetCompanyUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var company = request.Company;

        var users = await _companyUserRepository.GetAllAsync(company.Id);

        return Result.Status(OperationStatus.Ok, users.AsEnumerable());
    }
}