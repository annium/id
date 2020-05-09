using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Commands.Apps;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.CommandHandlers
{
    internal class AppCommandHandler :
        ICommandHandler<CreateAppCommand, Guid>,
        ICommandHandler<UpdateAppCommand>,
        ICommandHandler<SetAppOwnerCommand>,
        ICommandHandler<UpdateAppApiTokenCommand, Guid>,
        ICommandHandler<DeleteAppCommand>
    {
        private readonly IAppRepository appRepository;

        public AppCommandHandler(
            IAppRepository appRepository
        )
        {
            this.appRepository = appRepository;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            CreateAppCommand request,
            CancellationToken cancellationToken
        )
        {
            var app = new App(request.MyId, request.Name, Guid.NewGuid());

            app = await appRepository.CreateAsync(app);

            return Result.Status(OperationStatus.OK, app.Id);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UpdateAppCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to update app");

            app.Name = request.Name;

            await appRepository.UpdateAsync(app);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            SetAppOwnerCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;
            var newOwner = request.NewOwner;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to change app owner");

            app.OwnerId = newOwner.Id;

            await appRepository.UpdateAsync(app);

            return Result.Status(OperationStatus.OK);

        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            UpdateAppApiTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error($"Need to be application owner to update app api token");

            var apiToken = Guid.NewGuid();
            await appRepository.UpdateApiTokenAsync(app.Id, Guid.NewGuid());

            return Result.Status(OperationStatus.OK, apiToken);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteAppCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to update app api token");

            await appRepository.DeleteByIdAsync(app.Id);

            return Result.Status(OperationStatus.OK);
        }
    }
}