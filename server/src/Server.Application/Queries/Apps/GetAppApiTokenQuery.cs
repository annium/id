using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Server.Db.Repositories;
using Server.Domain.Queries.Apps;

namespace Server.Application.Queries.Apps;

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
        Field(c => c.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        Field(c => c.App).LoadWith(ctx => appRepository.TryGetByIdAsync(ctx.Root.AppId));
    }
}