using System.IO;
using System.Security.Cryptography;
using Annium.Data.Operations;
using Annium.Identity.Tokens;
using Annium.Identity.Tokens.Jwt;
using Annium.Logging;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Annium.Id.Core.Internal;

internal class TokenReader : ITokenReader, ILogSubject
{
    /// <summary>Clock skew allowed when the lifetime is validated.</summary>
    private static readonly Duration _expirationWindow = Duration.FromSeconds(5);

    public ILogger Logger { get; }
    private readonly JwtReader _reader;

    public TokenReader(AuthOptions options, ITimeProvider timeProvider, ILogger logger)
    {
        var securityKey = RSA.Create().ImportPem(File.ReadAllText(options.PublicKeyFile)).GetKey();
        _reader = new JwtReader(
            new JwtTokensOptions
            {
                SigningKey = securityKey,
                Algorithm = SecurityAlgorithms.RsaSha256,
                Issuer = Constants.Issuer,
                Audience = options.Audience.ToString(),
                ExpirationWindow = _expirationWindow,
                Lifetime = options.AccessTokenLifeTime,
            },
            timeProvider
        );
        Logger = logger;
    }

    /// <summary>
    /// Validates the token through <see cref="JwtReader"/> and unpacks the id claim from the principal
    /// it returns.
    /// </summary>
    /// <remarks>
    /// The reader used to hand back the SecurityToken* exception it caught, leaving eleven of them to be
    /// matched here before a status could be named. It now names the status itself, so this method has
    /// one job left: the id claim, which is this library's own concern and not JWT's.
    /// </remarks>
    /// <param name="tokenString">The raw JWT.</param>
    /// <param name="options">Which validations to apply.</param>
    /// <returns>The token, or the reason it was rejected.</returns>
    public IStatusResult<TokenReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options)
    {
        var result = _reader.Read(
            tokenString,
            new JwtReadOverrides(
                ValidateAudience: options.ValidateAudience,
                ValidateLifetime: options.ValidateExpiration
            )
        );

        if (result.Status is not TokenReadStatus.Ok)
            return Fail(result.Status, result.Error ?? "Token validation failed");

        // Ok guarantees a principal; the id claim is ours to require, and its absence means a token that
        // validated fine and still cannot be used - InvalidClaims says exactly that
        var idClaim = result.Claims!.FindFirst(Claims.Id);
        if (idClaim is null)
            return Fail(TokenReadStatus.InvalidClaims, "Token id is missing");

        return Result.Status(TokenReadStatus.Ok, Serializer.Deserialize<IdToken>(idClaim.Value));
    }

    /// <summary>
    /// Builds a failed result, logging the reasons that point at configuration rather than at the token.
    /// </summary>
    /// <param name="status">Why the token was rejected.</param>
    /// <param name="error">The message to carry.</param>
    /// <returns>The failed result.</returns>
    private IStatusResult<TokenReadStatus, IdToken> Fail(TokenReadStatus status, string error)
    {
        // an expired or wrong-audience token is a client's problem and says nothing worth logging; a
        // signature we cannot verify, or a failure nobody classified, usually means our own keys or
        // configuration are wrong
        if (status is TokenReadStatus.InvalidSignature or TokenReadStatus.Unknown)
            this.Error(error);

        return Result.Status<TokenReadStatus, IdToken>(status, null!).Error(error);
    }
}
