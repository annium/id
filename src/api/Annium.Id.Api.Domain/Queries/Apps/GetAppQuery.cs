using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.Apps
{
    public class GetAppQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; } = null!;

        public GetAppQuery(
            Guid appId
        )
        {
            AppId = appId;
        }
    }
}