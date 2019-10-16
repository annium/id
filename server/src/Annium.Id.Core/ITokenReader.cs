using Annium.Data.Operations;

namespace Annium.Id.Core
{
    public interface ITokenReader
    {
        IStatusResult<TokenReadStatus, T> ReadToken<T>(string tokenString);
    }
}