using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Commands.Roles;
using Server.Domain.Models;

namespace Server.Application.CommandHandlers;

internal class RoleCommandHandler :
    ICommandHandler<CreateRoleCommand, Guid>,
    ICommandHandler<UpdateRoleCommand>,
    ICommandHandler<AddClaimToRoleCommand>,
    ICommandHandler<DeleteClaimFromRoleCommand>,
    ICommandHandler<DeleteRoleCommand>
{
    private readonly IAppRepository _appRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRoleClaimRepository _roleClaimRepository;

    public RoleCommandHandler(
        IAppRepository appRepository,
        IRoleRepository roleRepository,
        IRoleClaimRepository roleClaimRepository
    )
    {
        _appRepository = appRepository;
        _roleRepository = roleRepository;
        _roleClaimRepository = roleClaimRepository;
    }

    public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
        CreateRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = request.App;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error("Need to be application owner to create role");

        var role = new Role(
            app.Id,
            request.Key,
            request.Name,
            Array.Empty<ClaimValue>()
        );

        role = await _roleRepository.CreateAsync(role);

        return Result.Status(OperationStatus.Ok, role.Id);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UpdateRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to update role");

        if (request.Key != role.Key && await _roleRepository.TryFindByKeyAsync(app.Id, request.Key) != null)
            return Result.Status(OperationStatus.Conflict).Error($"Role key {request.Key} is already used");

        role.Key = request.Key;
        role.Name = request.Name;

        await _roleRepository.UpdateAsync(role);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddClaimToRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add claim to role");

        if (claim.AppId != app.Id)
            return Result.Status(OperationStatus.Forbidden).Error("Claim belongs to another application");

        var roleClaim = new RoleClaim(role.Id, claim.Id, request.Value);

        await _roleClaimRepository.SaveAsync(roleClaim);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteClaimFromRoleCommand request,
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
            return Result.Status(OperationStatus.Forbidden).Error("Claim belongs to another application");

        await _roleClaimRepository.DeleteByIdAsync(role.Id, claim.Id);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var role = request.Role;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete role");

        await _roleRepository.DeleteByIdAsync(role.Id);

        return Result.Status(OperationStatus.Ok);
    }
}