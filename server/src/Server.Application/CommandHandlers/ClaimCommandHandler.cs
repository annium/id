using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Commands.Claims;
using Server.Domain.Models;

namespace Server.Application.CommandHandlers;

internal class ClaimCommandHandler :
    ICommandHandler<CreateClaimCommand, Guid>,
    ICommandHandler<UpdateClaimCommand>,
    ICommandHandler<DeleteClaimCommand>
{
    private readonly IAppRepository _appRepository;
    private readonly IClaimRepository _claimRepository;

    public ClaimCommandHandler(
        IAppRepository appRepository,
        IClaimRepository claimRepository
    )
    {
        _appRepository = appRepository;
        _claimRepository = claimRepository;
    }

    public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
        CreateClaimCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = request.App;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error("Need to be application owner to create claim");

        var claim = new Claim(
            app.Id,
            request.Key,
            request.Name
        );

        claim = await _claimRepository.CreateAsync(claim);

        return Result.Status(OperationStatus.Ok, claim.Id);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UpdateClaimCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Claim.AppId);
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to update claim");

        if (request.Key != claim.Key && await _claimRepository.TryFindByKeyAsync(app.Id, request.Key) != null)
            return Result.Status(OperationStatus.Conflict).Error($"Claim key {request.Key} is already used");

        claim.Key = request.Key;
        claim.Name = request.Name;

        await _claimRepository.UpdateAsync(claim);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteClaimCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Claim.AppId);
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete claim");

        await _claimRepository.DeleteByIdAsync(claim.Id);

        return Result.Status(OperationStatus.Ok);
    }
}