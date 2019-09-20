using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.AppUsers;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.CommandHandlers
{
    internal class AppUserCommandHandler : ICommandHandler<AddRoleToUserCommand>, ICommandHandler<DeleteRoleFromUserCommand>, ICommandHandler<AddClaimToUserCommand>, ICommandHandler<DeleteClaimFromUserCommand>
    {
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserClaimRepository userClaimRepository;

        public AppUserCommandHandler(
            IUserRoleRepository userRoleRepository,
            IUserClaimRepository userClaimRepository
        )
        {
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            AddRoleToUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;
            var user = request.User;
            var role = request.Role;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add role to user");

            if (role.AppId != app.Id)
                return Result.Status(OperationStatus.Forbidden).Error("Role belongs to another application");

            var userRole = new UserRole(user.Id, role.Id);
            await userRoleRepository.SaveAsync(userRole);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteRoleFromUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;
            var user = request.User;
            var role = request.Role;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete role from user");

            if (role.AppId != app.Id)
                return Result.Status(OperationStatus.Forbidden).Error("Role belongs to another application");

            await userRoleRepository.DeleteByIdAsync(user.Id, role.Id);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            AddClaimToUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;
            var user = request.User;
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add claim to user");

            if (claim.AppId != app.Id)
                return Result.Status(OperationStatus.Forbidden).Error("Claim belongs to another application");

            var userClaim = new UserClaim(user.Id, claim.Id, request.Value);
            await userClaimRepository.SaveAsync(userClaim);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteClaimFromUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;
            var user = request.User;
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete claim from user");

            if (claim.AppId != app.Id)
                return Result.Status(OperationStatus.Forbidden).Error("Claim belongs to another application");

            await userClaimRepository.DeleteByIdAsync(user.Id, claim.Id);

            return Result.Status(OperationStatus.OK);
        }
    }
}