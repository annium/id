using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Api.Domain.Queries.Apps;
using Annium.Id.Core;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Queries.Apps;

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
        Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
    }
}