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

        public async Task<IResult<TokensResponse>> Login(string login, string password)
        {
            var response = await _serverApi.Public.Client().Login.LogIn(_config.AppId, new LogInRequestBody { Login = login, Password = password });

            if (response.IsSuccess)
                await _tokenStore.SetAsync(response.Data.Data);
            else
                await _tokenStore.ClearAsync();

            return response.Data;
        }
    }

    public interface ILoginService : IApiService
    {
        Task<IResult<TokensResponse>> Login(string login, string password);
    }
}