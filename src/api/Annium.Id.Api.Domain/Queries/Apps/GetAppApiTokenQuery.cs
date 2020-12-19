using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Apps
{
    public class GetAppApiTokenQuery : IQuery
    {
        public Guid AppId { get; }
        public Guid MyId { get; private set; }
        public App App { get; private set; } = null!;

        public GetAppApiTokenQuery(
            Guid appId
        )
        {
            AppId = appId;
        }
    }
}