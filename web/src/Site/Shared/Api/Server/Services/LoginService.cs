using System;
using System.Threading.Tasks;
using Annium.Blazor.Net;
using Annium.Data.Operations;
using Server.ViewModels.Requests.Login;
using Server.ViewModels.Responses.Login;
using Site.Shared.Api.Server.Clients;
using Site.Shared.Stores;

namespace Site.Shared.Api.Server.Services;

internal class LoginService : ILoginService
{
    private readonly Configuration _config;
    private readonly IServerApi _serverApi;
    private readonly ITokenStore _tokenStore;

    public LoginService(Configuration config, IServerApi serverApi, ITokenStore tokenStore)
    {
        _config = config;
        _serverApi = serverApi;
        _tokenStore = tokenStore;
    }

    public async Task<IResult<TokensResponse>> LogInAsync(string login, string password)
    {
        var response = await _serverApi
            .Public.Client()
            .Login.LogInAsync(
                _config.AppId,
                new LogInRequestBody { Login = login, Password = password },
                Result.New(new TokensResponse()).Error("Failed to log in")
            );

        if (response.IsOk)
            _tokenStore.Set(response.Data);
        else
            _tokenStore.Clear();

        return response;
    }

    public Task<IResult> LogOutAsync() =>
        _serverApi.Private.Client().Login.LogOutAsync(_config.AppId, Result.New().Error("Failed to log out"));

    public async Task<IResult<TokensResponse>> UpdateTokenAsync(Guid refreshToken)
    {
        var response = await _serverApi
            .Private.Client()
            .Login.UpdateTokenAsync(
                _config.AppId,
                refreshToken,
                Result.New(new TokensResponse()).Error("Failed to update token")
            );

        if (response.IsOk)
            _tokenStore.Set(response.Data);
        else
            _tokenStore.Clear();

        return response;
    }
}

public interface ILoginService : IApiService
{
    Task<IResult<TokensResponse>> LogInAsync(string login, string password);
    Task<IResult> LogOutAsync();
    Task<IResult<TokensResponse>> UpdateTokenAsync(Guid refreshToken);
}
