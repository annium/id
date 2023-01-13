using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Server.Db.Repositories;
using Server.Domain.Queries.Apps;

namespace Server.Application.Queries.Apps;

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
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}