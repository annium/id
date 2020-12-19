using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Api.Application.Queries.Apps;
using Annium.Id.Api.Domain.Queries.Apps;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.QueryHandlers
{
    internal class AppQueryHandler :
        IQueryHandler<FindAppsQuery, IEnumerable<App>>,
        IQueryHandler<ListMyAppsQuery, IEnumerable<App>>,
        IQueryHandler<GetAppQuery, App>,
        IQueryHandler<GetAppApiTokenQuery, Guid>
    {
        private readonly IAppRepository _appRepository;

        public AppQueryHandler(
            IAppRepository appRepository
        )
        {
            _appRepository = appRepository;
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<App>>> HandleAsync(
            FindAppsQuery request,
            CancellationToken cancellationToken
        )
        {
            var apps = await _appRepository.FindAllAsync(request.Query);

            return Result.Status(OperationStatus.Ok, apps.AsEnumerable());
        }

        public async Task<IStatusResult<OperationStatus, IEnumerable<App>>> HandleAsync(
            ListMyAppsQuery request,
            CancellationToken cancellationToken
        )
        {
            var apps = await _appRepository.FindMyAsync(request.User.Id);

            return Result.Status(OperationStatus.Ok, apps.AsEnumerable());
        }

        public Task<IStatusResult<OperationStatus, App>> HandleAsync(
            GetAppQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Result.Status(OperationStatus.Ok, request.App));
        }

        public Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            GetAppApiTokenQuery request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;

            if (myId != app.OwnerId)
                return Task.FromResult(Result.Status(OperationStatus.Forbidden, Guid.Empty)
                    .Error($"Need to be application owner to get app api token"));

            return Task.FromResult(Result.Status(OperationStatus.Ok, app.ApiToken));
        }
    }
}