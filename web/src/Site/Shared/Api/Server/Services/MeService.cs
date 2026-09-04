using System.Threading.Tasks;
using Annium.Blazor.Net;
using Annium.Data.Operations;
using Server.ViewModels.Responses.Me;
using Site.Shared.Api.Server.Clients;

namespace Site.Shared.Api.Server.Services;

internal class MeService : IMeService
{
    private readonly IServerApi _serverApi;

    public MeService(IServerApi serverApi)
    {
        _serverApi = serverApi;
    }

    public Task<IResult<MeResponse>> GetMeAsync() =>
        _serverApi
            .Private.Client()
            .Me.GetMeAsync(Result.Create(new MeResponse()).Error("Failed to load personal information"));
}

public interface IMeService : IApiService
{
    Task<IResult<MeResponse>> GetMeAsync();
}
