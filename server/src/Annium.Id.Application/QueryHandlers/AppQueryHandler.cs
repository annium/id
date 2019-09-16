using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Application.Queries.Apps;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.QueryHandlers
{
    internal class AppQueryHandler : IQueryHandler<ListAppsQuery, IEnumerable<App>>, IQueryHandler<GetAppQuery, App>, IQueryHandler<GetAppApiTokenQuery, Guid>
    {
        private readonly IAppRepository appRepository;

        public AppQueryHandler(
            IAppRepository appRepository
        )
        {
            this.appRepository = appRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<App>>> HandleAsync(
            ListAppsQuery request,
            CancellationToken cancellationToken
        )
        {
            var apps = await appRepository.GetAllAsync();

            return Result.Status(OperationStatus.OK, apps.AsEnumerable());
        }

        public Task<IStatusResult<OperationStatus, App>> HandleAsync(
            GetAppQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Result.Status(OperationStatus.OK, request.App));
        }

        public Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            GetAppApiTokenQuery request,
            CancellationToken cancellationToken
        )
        {
            var userId = request.UserId;
            var app = request.App;

            if (userId != app.OwnerId)
                return Task.FromResult(Result.Status(OperationStatus.Forbidden, Guid.Empty).Error($"Need to be application owner to get app api token"));

            return Task.FromResult(Result.Status(OperationStatus.OK, app.ApiToken));
        }
    }
}