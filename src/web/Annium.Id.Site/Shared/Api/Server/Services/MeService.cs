using System.Threading.Tasks;
using Annium.Blazor.Net;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Responses.Me;

namespace Annium.Id.Site.Shared.Api.Server.Services;

internal class MeService : IMeService
{
    private readonly IServerApi _serverApi;

    public MeService(
        IServerApi serverApi
    )
    {
        _serverApi = serverApi;
    }

    public Task<IResult<MeResponse>> GetMe() => _serverApi.Private.Client().Me.GetMe();
}

public interface IMeService : IApiService
{
    Task<IResult<MeResponse>> GetMe();
}