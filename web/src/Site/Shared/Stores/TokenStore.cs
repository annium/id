using Annium.Blazor.State;
using Server.ViewModels.Responses.Login;

namespace Site.Shared.Stores;

internal class TokenStore : StateBase, ITokenStore
{
    private const string Key = "tokens";
    private readonly ILocalStorage _storage;

    public TokenStore(ILocalStorage storage)
    {
        _storage = storage;
    }

    public TokensResponse? Get()
    {
        if (!_storage.HasKey(Key))
            return null;

        return _storage.Get<TokensResponse>(Key);
    }

    public void Set(TokensResponse tokens) => _storage.Set(Key, tokens);


    public void Clear() => _storage.Remove(Key);
}

public interface ITokenStore
{
    TokensResponse? Get();
    void Set(TokensResponse tokens);
    void Clear();
}