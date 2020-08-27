using System.Threading.Tasks;
using Annium.Blazor.Storage;
using Annium.Id.Api.ViewModels.Responses.Login;

namespace Annium.Id.Site.Shared.Stores
{
    internal class TokenStore : ITokenStore
    {
        private const string Key = "tokens";
        private readonly ILocalStorage _storage;

        public TokenStore(ILocalStorage storage)
        {
            _storage = storage;
        }

        public async ValueTask<TokensResponse?> GetAsync()
        {
            if (!await _storage.HasKeyAsync(Key))
                return null;

            return await _storage.GetAsync<TokensResponse>(Key);
        }

        public async ValueTask SetAsync(TokensResponse tokens)
        {
            await _storage.SetAsync(Key, tokens);
        }

        public async ValueTask ClearAsync()
        {
            await _storage.RemoveAsync(Key);
        }
    }

    public interface ITokenStore : IStore
    {
        ValueTask<TokensResponse?> GetAsync();
        ValueTask SetAsync(TokensResponse tokens);
        ValueTask ClearAsync();
    }
}