using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Apps
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

    internal class GetAppApiTokenQueryValidator : Validator<GetAppApiTokenQuery>
    {
        public GetAppApiTokenQueryValidator()
        {
            Field(c => c.AppId).Required();
        }
    }

    internal class GetAppApiTokenQueryComposer : Composer<GetAppApiTokenQuery>
    {
        public GetAppApiTokenQueryComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}