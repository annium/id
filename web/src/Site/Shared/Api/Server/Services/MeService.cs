using System.Threading.Tasks;
using Annium.Blazor.Net;
using Annium.Data.Operations;
using Server.ViewModels.Responses.Me;
using Site.Shared.Api.Server.Clients;

namespace Site.Shared.Api.Server.Services;

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