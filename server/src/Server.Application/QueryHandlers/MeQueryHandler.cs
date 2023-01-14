using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Queries;
using Annium.Data.Operations;
using Annium.Id.Core;
using Server.Application.Tools;
using Server.Domain.Models;
using Server.Domain.Queries.Me;

namespace Server.Application.QueryHandlers;

internal class MeQueryHandler :
    IQueryHandler<GetMeQuery, User>,
    IQueryHandler<GetMyTokenQuery, IdToken>
{
    private readonly ITokenGenerator _tokenGenerator;

    public MeQueryHandler(
        ITokenGenerator tokenGenerator
    )
    {
        _tokenGenerator = tokenGenerator;
    }

    public Task<IStatusResult<OperationStatus, User>> HandleAsync(
        GetMeQuery request,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(Result.Status(OperationStatus.Ok, request.User));
    }

    public async Task<IStatusResult<OperationStatus, IdToken>> HandleAsync(
        GetMyTokenQuery request,
        CancellationToken ct
    )
    {
        var token = await _tokenGenerator.GenerateToken(request.Login);

        return Result.Status(OperationStatus.Ok, token);
    }
}