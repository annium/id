using Annium.Data.Operations;

namespace Annium.Id.Core
{
    public interface ITokenParser
    {
        IStatusResult<TokenParseStatus, IdBaseToken> ParseToken(string tokenString);
    }
}