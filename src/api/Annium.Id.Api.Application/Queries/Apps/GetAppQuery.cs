using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Apps;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Apps
{
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