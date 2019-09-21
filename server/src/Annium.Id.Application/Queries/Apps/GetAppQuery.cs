using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Apps
{
    public class GetAppQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; }

        public GetAppQuery(
            Guid appId
        )
        {
            AppId = appId;
        }
    }

    internal class GetAppQueryValidator : Validator<GetAppQuery>
    {
        public GetAppQueryValidator()
        {
            Field(c => c.AppId).Required();
        }
    }

    internal class GetAppQueryComposer : Composer<GetAppQuery>
    {
        public GetAppQueryComposer(
            IAppRepository appRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}