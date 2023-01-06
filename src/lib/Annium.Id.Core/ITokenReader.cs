using Annium.Data.Operations;

namespace Annium.Id.Core;

public interface ITokenReader
{
    IStatusResult<TokenReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options);
}