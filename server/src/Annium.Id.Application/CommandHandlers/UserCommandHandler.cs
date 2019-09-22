using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.Users;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.CommandHandlers
{
    internal class UserCommandHandler : ICommandHandler<AddRoleToUserCommand>, ICommandHandler<DeleteRoleFromUserCommand>, ICommandHandler<AddClaimToUserCommand>, ICommandHandler<DeleteClaimFromUserCommand>
    {
        private readonly IAppRepository appRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserClaimRepository userClaimRepository;

        public UserCommandHandler(
            IAppRepository appRepository,
            IUserRoleRepository userRoleRepository,
            IUserClaimRepository userClaimRepository
        )
        {
            this.appRepository = appRepository;
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            AddRoleToUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Role.AppId);
            var user = request.User;
            var role = request.Role;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add role to user");

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
            var app = await appRepository.GetByIdAsync(request.Role.AppId);
            var user = request.User;
            var role = request.Role;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete role from user");

            await userRoleRepository.DeleteByIdAsync(user.Id, role.Id);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            AddClaimToUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Claim.AppId);
            var user = request.User;
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to add claim to user");

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
            var app = await appRepository.GetByIdAsync(request.Claim.AppId);
            var user = request.User;
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be application owner to delete claim from user");

            await userClaimRepository.DeleteByIdAsync(user.Id, claim.Id);

            return Result.Status(OperationStatus.OK);
        }
    }
}