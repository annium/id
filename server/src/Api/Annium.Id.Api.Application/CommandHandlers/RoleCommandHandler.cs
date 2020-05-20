using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Commands.Roles;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.CommandHandlers
{
    internal class RoleCommandHandler :
        ICommandHandler<CreateRoleCommand, Guid>,
        ICommandHandler<UpdateRoleCommand>,
        ICommandHandler<AddClaimToRoleCommand>,
        ICommandHandler<DeleteClaimFromRoleCommand>,
        ICommandHandler<DeleteRoleCommand>
    {
        private readonly IAppRepository appRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRoleClaimRepository roleClaimRepository;

        public RoleCommandHandler(
            IAppRepository appRepository,
            IRoleRepository roleRepository,
            IRoleClaimRepository roleClaimRepository
        )
        {
            this.appRepository = appRepository;
            this.roleRepository = roleRepository;
            this.roleClaimRepository = roleClaimRepository;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            CreateRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error($"Need to be application owner to create role");

            var role = new Role(
                app.Id,
                request.Key,
                request.Name,
                Array.Empty<ClaimValue>()
            );

            role = await roleRepository.CreateAsync(role);

            return Result.Status(OperationStatus.OK, role.Id);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UpdateRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Role.AppId);
            var role = request.Role;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to update role");

            if (request.Key != role.Key && (await roleRepository.FindByKeyAsync(app.Id, request.Key)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Role key {request.Key} is already used");

            role.Key = request.Key;
            role.Name = request.Name;

            await roleRepository.UpdateAsync(role);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            AddClaimToRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Role.AppId);
            var role = request.Role;
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to add claim to role");

            if (claim.AppId != app.Id)
                return Result.Status(OperationStatus.Forbidden).Error("Claim belongs to another application");

            var roleClaim = new RoleClaim(role.Id, claim.Id, request.Value);

            await roleClaimRepository.SaveAsync(roleClaim);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteClaimFromRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Role.AppId);
            var role = request.Role;
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to delete claim from role");

            if (claim.AppId != app.Id)
                return Result.Status(OperationStatus.Forbidden).Error("Claim belongs to another application");

            await roleClaimRepository.DeleteByIdAsync(role.Id, claim.Id);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Role.AppId);
            var role = request.Role;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to delete role");

            await roleRepository.DeleteByIdAsync(role.Id);

            return Result.Status(OperationStatus.OK);
        }
    }
}