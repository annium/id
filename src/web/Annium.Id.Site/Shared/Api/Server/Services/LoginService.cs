using System;
using System.Threading.Tasks;
using Annium.Blazor.Net;
using Annium.Data.Operations;
using Annium.Id.Api.ViewModels.Requests.Login;
using Annium.Id.Api.ViewModels.Responses.Login;
using Annium.Id.Site.Shared.Stores;

namespace Annium.Id.Site.Shared.Api.Server.Services
{
    internal class LoginService : ILoginService
    {
        private readonly Configuration _config;
        private readonly IServerApi _serverApi;
        private readonly ITokenStore _tokenStore;

        public LoginService(
            Configuration config,
            IServerApi serverApi,
            ITokenStore tokenStore
        )
        {
            _config = config;
            _serverApi = serverApi;
            _tokenStore = tokenStore;
        }

        public async Task<IResult<TokensResponse>> LogIn(string login, string password)
        {
            var response = await _serverApi.Public.Client().Login.LogIn(_config.AppId, new LogInRequestBody { Login = login, Password = password });

            if (response.IsOk)
                await _tokenStore.SetAsync(response.Data);
            else
                await _tokenStore.ClearAsync();

            return response;
        }

        public Task<IResult> LogOut() => _serverApi.Private.Client().Login.LogOut(_config.AppId);

        public async Task<IResult<TokensResponse>> LogIn(Guid refreshToken)
        {
            var response = await _serverApi.Private.Client().Login.UpdateToken(_config.AppId, refreshToken);

            if (response.IsOk)
                await _tokenStore.SetAsync(response.Data);
            else
                await _tokenStore.ClearAsync();

            return response;
        }
    }

    public interface ILoginService : IApiService
    {
        Task<IResult<TokensResponse>> LogIn(string login, string password);
        Task<IResult> LogOut();
        Task<IResult<TokensResponse>> LogIn(Guid refreshToken);
    }
}