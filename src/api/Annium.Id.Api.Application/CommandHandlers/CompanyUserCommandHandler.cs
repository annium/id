using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Api.Domain.Commands.CompanyUsers;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.CommandHandlers;

internal class CompanyUserCommandHandler :
    ICommandHandler<AddUserToCompanyCommand>,
    ICommandHandler<AddCompanyRoleToCompanyUserCommand>,
    ICommandHandler<DeleteCompanyRoleFromCompanyUserCommand>,
    ICommandHandler<AddCompanyClaimToCompanyUserCommand>,
    ICommandHandler<DeleteCompanyClaimFromCompanyUserCommand>,
    ICommandHandler<DeleteUserFromCompanyCommand>
{
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly ICompanyUserRoleRepository _companyUserRoleRepository;
    private readonly ICompanyUserClaimRepository _companyUserClaimRepository;

    public CompanyUserCommandHandler(
        ICompanyUserRepository companyUserRepository,
        ICompanyUserRoleRepository companyUserRoleRepository,
        ICompanyUserClaimRepository companyUserClaimRepository
    )
    {
        _companyUserRepository = companyUserRepository;
        _companyUserRoleRepository = companyUserRoleRepository;
        _companyUserClaimRepository = companyUserClaimRepository;
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddUserToCompanyCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;
        var user = request.User;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to add user to company");

        var companyUser = new CompanyUser(company.Id, user.Id);
        await _companyUserRepository.SaveAsync(companyUser);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddCompanyRoleToCompanyUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;
        var user = request.User;
        var role = request.Role;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to add company role to company user");

        if (await _companyUserRepository.GetByIdAsync(company.Id, user.Id) is null)
            return Result.Status(OperationStatus.Forbidden).Error("User is not company member");

        var companyUserRole = new CompanyUserRole(company.Id, user.Id, role.Id);
        await _companyUserRoleRepository.SaveAsync(companyUserRole);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteCompanyRoleFromCompanyUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;
        var user = request.User;
        var role = request.Role;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to delete company role from company user");

        if (await _companyUserRepository.GetByIdAsync(company.Id, user.Id) is null)
            return Result.Status(OperationStatus.Forbidden).Error("User is not company member");

        await _companyUserRoleRepository.DeleteByIdAsync(company.Id, user.Id, role.Id);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddCompanyClaimToCompanyUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;
        var user = request.User;
        var claim = request.Claim;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to add company claim to company user");

        if (await _companyUserRepository.GetByIdAsync(company.Id, user.Id) is null)
            return Result.Status(OperationStatus.Forbidden).Error("User is not company member");

        var companyUserClaim = new CompanyUserClaim(company.Id, user.Id, claim.Id, request.Value);
        await _companyUserClaimRepository.SaveAsync(companyUserClaim);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteCompanyClaimFromCompanyUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;
        var user = request.User;
        var claim = request.Claim;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to delete company claim from company user");

        if (await _companyUserRepository.GetByIdAsync(company.Id, user.Id) is null)
            return Result.Status(OperationStatus.Forbidden).Error("User is not company member");

        await _companyUserClaimRepository.DeleteByIdAsync(company.Id, user.Id, claim.Id);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteUserFromCompanyCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;
        var user = request.User;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to delete user from company");

        if (await _companyUserRepository.GetByIdAsync(company.Id, user.Id) is null)
            return Result.Status(OperationStatus.Forbidden).Error("User is not company member");

        await _companyUserRepository.DeleteByIdAsync(company.Id, user.Id);

        return Result.Status(OperationStatus.Ok);
    }
}