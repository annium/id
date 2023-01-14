using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Commands.Users;
using Server.Domain.Models;

namespace Server.Application.CommandHandlers;

internal class UserCommandHandler :
    ICommandHandler<AddRoleToUserCommand>,
    ICommandHandler<DeleteRoleFromUserCommand>,
    ICommandHandler<AddClaimToUserCommand>,
    ICommandHandler<DeleteClaimFromUserCommand>
{
    private readonly IAppRepository _appRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUserClaimRepository _userClaimRepository;

    public UserCommandHandler(
        IAppRepository appRepository,
        IUserRoleRepository userRoleRepository,
        IUserClaimRepository userClaimRepository
    )
    {
        _appRepository = appRepository;
        _userRoleRepository = userRoleRepository;
        _userClaimRepository = userClaimRepository;
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddRoleToUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var user = request.User;
        var role = request.Role;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add role to user");

        var userRole = new UserRole(user.Id, role.Id);
        await _userRoleRepository.SaveAsync(userRole);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteRoleFromUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Role.AppId);
        var user = request.User;
        var role = request.Role;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete role from user");

        await _userRoleRepository.DeleteByIdAsync(user.Id, role.Id);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        AddClaimToUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Claim.AppId);
        var user = request.User;
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add claim to user");

        var userClaim = new UserClaim(user.Id, claim.Id, request.Value);
        await _userClaimRepository.SaveAsync(userClaim);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        DeleteClaimFromUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var app = await _appRepository.GetByIdAsync(request.Claim.AppId);
        var user = request.User;
        var claim = request.Claim;

        if (myId != app.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete claim from user");

        await _userClaimRepository.DeleteByIdAsync(user.Id, claim.Id);

        return Result.Status(OperationStatus.Ok);
    }
}