using Annium.Data.Operations;
using Annium.Identity.Tokens.Jwt;

namespace Annium.Id.Core;

public interface ITokenReader
{
    IStatusResult<JwtReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options);
}