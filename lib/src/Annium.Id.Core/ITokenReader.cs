using Annium.Data.Operations;
using Annium.Identity.Tokens;

namespace Annium.Id.Core;

/// <summary>
/// Reads an <see cref="IdToken"/> out of a signed JWT.
/// </summary>
public interface ITokenReader
{
    /// <summary>
    /// Validates the token and unpacks the id claim from it.
    /// </summary>
    /// <param name="tokenString">The raw JWT.</param>
    /// <param name="options">Which validations to apply.</param>
    /// <returns>
    /// The token on <see cref="TokenReadStatus.Ok"/>, otherwise the reason it was rejected. The status
    /// comes from Annium.Identity.Tokens rather than a local enum: what used to be one catch-all
    /// failure is now the specific one - expired, wrong audience, bad signature - and callers that
    /// only need the coarse distinction can still make it.
    /// </returns>
    IStatusResult<TokenReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options);
}
