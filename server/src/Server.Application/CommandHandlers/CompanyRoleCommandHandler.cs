using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Commands.CompanyRoles;
using Server.Domain.Models;

namespace Server.Application.CommandHandlers;

internal class CompanyRoleCommandHandler :
    ICommandHandler<CreateCompanyRoleCommand, Guid>,
    ICommandHandler<UpdateCompanyRoleCommand>,
    ICommandHandler<AddCompanyClaimToCompanyRoleCommand>,
    ICommandHandler<DeleteCompanyClaimFromCompanyRoleCommand>,
    ICommandHandler<DeleteCompanyRoleCommand>
{
    private readonly IAppRepository _appRepository;
    private readonly ICompanyRoleRepository _companyRoleRepository;
    private readonly ICompanyRoleClaimRepository _companyRoleClaimRepository;

    public CompanyRoleCommandHandler(
        IAppRepository appRepository,
        ICompanyRoleRepository companyRoleRepository,
        ICompanyRoleClaimRepository companyRoleClaimRepository
    )
    {
        _appRepository = appRepository;
        _companyRoleRepository = companyRoleRepository;
        _companyRoleClaimRepository = companyRoleClaimRepository;
    }

    public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
        CreateCompanyRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = request.App;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error("Need to be application owner to create company role");

        var role = new CompanyRole(app, request.Key, request.Name);

        role = await _companyRoleRepository.CreateAsync(role);

        return Result.Status(OperationStatus.Ok, role.Id);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UpdateCompanyRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to create company role");

        if (request.Key != role.Key && await _companyRoleRepository.TryFindByKeyAsync(app.Id, request.Key) != null)
            return Result.Status(OperationStatus.Conflict).Error($"Company role key {request.Key} is already used");

        role.Update(request.Key, request.Name);

        await _companyRoleRepository.UpdateAsync(role);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddCompanyClaimToCompanyRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add company claim to company role");

        if (claim.AppId != app.Id)
            return Result.Status(OperationStatus.Forbidden).Error("Company claim belongs to another application");

        var roleCompanyClaim = new CompanyRoleClaim(role, claim, request.Value);

        await _companyRoleClaimRepository.SaveAsync(roleCompanyClaim);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteCompanyClaimFromCompanyRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete claim from role");

        if (claim.AppId != app.Id)
            return Result.Status(OperationStatus.Forbidden).Error("CompanyClaim belongs to another application");

        await _companyRoleClaimRepository.DeleteByIdAsync(role.Id, claim.Id);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteCompanyRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete role");

        await _companyRoleRepository.DeleteByIdAsync(role.Id);

        return Result.Status(OperationStatus.Ok);
    }
}