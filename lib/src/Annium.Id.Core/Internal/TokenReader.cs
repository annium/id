using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Annium.Data.Operations;
using Annium.Identity.Tokens;
using Annium.Identity.Tokens.Jwt;
using Annium.Logging;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using OneOf;

namespace Annium.Id.Core.Internal;

internal class TokenReader : ITokenReader, ILogSubject
{
    public ILogger Logger { get; }
    private readonly RsaSecurityKey _securityKey;
    private readonly AuthOptions _options;
    private readonly ITimeProvider _timeProvider;

    public TokenReader(AuthOptions options, ITimeProvider timeProvider, ILogger logger)
    {
        _securityKey = RSA.Create().ImportPem(File.ReadAllText(options.PublicKeyFile)).GetKey();
        _options = options;
        _timeProvider = timeProvider;
        Logger = logger;
    }

    public IStatusResult<JwtReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options)
    {
        var now = _timeProvider.Now;
        var audience = options.ValidateAudience ? _options.Audience.ToString() : null;
        var expirationWindow = options.ValidateExpiration ? Duration.FromSeconds(5) : default(Duration?);
        var opts = JwtReader.GetValidationParameters(_securityKey, Constants.Issuer, audience, expirationWindow);

        var result = JwtReader.Read(tokenString, opts, now);

        return result.Data.Match(
            jwt =>
            {
                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Claims.Id);
                if (idClaim is null)
                    return Fail(JwtReadStatus.BadSource, "Token id is missing");

                var token = Serializer.Deserialize<IdToken>(idClaim.Value);

                return Result.Status(JwtReadStatus.Ok, token);
            },
            exception =>
                exception switch
                {
                    SecurityTokenDecompressionFailedException => FromFailure(result),
                    SecurityTokenEncryptionKeyNotFoundException => FromFailureWithLog(
                        result,
                        "Token encryption key not found"
                    ),
                    SecurityTokenDecryptionFailedException => FromFailure(result),
                    SecurityTokenNoExpirationException => FromFailure(result),
                    SecurityTokenExpiredException => FromFailure(result),
                    SecurityTokenNotYetValidException => FromFailure(result),
                    SecurityTokenInvalidLifetimeException => FromFailure(result),
                    SecurityTokenInvalidAudienceException => FromFailure(result),
                    SecurityTokenInvalidIssuerException => FromFailure(result),
                    SecurityTokenSignatureKeyNotFoundException => FromFailureWithLog(
                        result,
                        "Token signature key not found"
                    ),
                    SecurityTokenInvalidSignatureException => FromFailure(result),
                    _ => FromFailureWithLog(result, $"Token validation failed: {exception}"),
                }
        );
    }

    private IStatusResult<JwtReadStatus, IdToken> FromFailureWithLog(
        IStatusResult<JwtReadStatus, OneOf<JwtSecurityToken, Exception>> result,
        string message
    )
    {
        this.Error(message);

        return Result.Status<JwtReadStatus, IdToken>(result.Status, null!).Join(result);
    }

    private IStatusResult<JwtReadStatus, IdToken> FromFailure(
        IStatusResult<JwtReadStatus, OneOf<JwtSecurityToken, Exception>> result
    )
    {
        return Result.Status<JwtReadStatus, IdToken>(result.Status, null!).Join(result);
    }

    private IStatusResult<JwtReadStatus, IdToken> Fail(JwtReadStatus status, string error)
    {
        return Result.Status<JwtReadStatus, IdToken>(status, null!).Error(error);
    }
}
