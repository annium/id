using Annium.Data.Operations;

namespace Annium.Id.Core
{
    public interface ITokenParser
    {
        IStatusResult<TokenParseStatus, T> ParseToken<T>(string tokenString);
    }
}