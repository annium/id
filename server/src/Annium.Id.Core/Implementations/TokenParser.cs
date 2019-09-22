using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Annium.Data.Operations;
using Annium.Logging.Abstractions;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Annium.Id.Core.Implementations
{
    internal class TokenParser : ITokenParser
    {
        private readonly RsaSecurityKey signingKey;
        private readonly AuthorizationOptions options;
        private readonly ILogger logger;

        public TokenParser(
            AuthorizationOptions options,
            ILogger<TokenParser> logger
        )
        {
            using(var s = File.OpenRead(Path.Combine("keys", "public.key")))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }

            this.options = options;
            this.logger = logger;
        }

        public IStatusResult<TokenParseStatus, IdBaseToken> ParseToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return fail(TokenParseStatus.BadSource, "Token is not valid JWT");

            var tvp = new TokenValidationParameters();
            tvp.ClockSkew = Duration.FromSeconds(5).ToTimeSpan();
            tvp.IssuerSigningKey = signingKey;
            tvp.RequireExpirationTime = true;
            tvp.RequireSignedTokens = true;
            tvp.ValidateAudience = false;
            tvp.ValidAudiences = string.IsNullOrWhiteSpace(options.Audience) ?
                new [] { Constants.BaseAudience } :
                new [] { Constants.BaseAudience, options.Audience };
            tvp.ValidateIssuer = true;
            tvp.ValidIssuer = Constants.Issuer;
            tvp.ValidateIssuerSigningKey = true;
            tvp.ValidateLifetime = true;

            try
            {
                handler.ValidateToken(tokenString, tvp, out var securityToken);
                var jwt = (JwtSecurityToken) securityToken;

                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Claims.Id);
                if (idClaim == null)
                    return fail(TokenParseStatus.BadSource, "Token id is missing");

                var rawToken = Convert.FromBase64String(idClaim.Value);

                if (jwt.Audiences.Contains(Constants.BaseAudience))
                    return Result.Status(TokenParseStatus.Ok, LZ4MessagePackSerializer.Deserialize<IdBaseToken>(rawToken));

                return Result.Status<TokenParseStatus, IdBaseToken>(TokenParseStatus.Ok, LZ4MessagePackSerializer.Deserialize<IdAppToken>(rawToken));
            }
            catch (SecurityTokenDecompressionFailedException)
            {
                return fail(TokenParseStatus.Failed, "Token decompression failed");
            }
            catch (SecurityTokenEncryptionKeyNotFoundException)
            {
                logger.Error("Token encryption key not found");

                return fail(TokenParseStatus.Failed, "Token decryption failed");
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                return fail(TokenParseStatus.Failed, "Token decryption failed");
            }
            catch (SecurityTokenNoExpirationException)
            {
                return fail(TokenParseStatus.Failed, "Token has no expiration claim");
            }
            catch (SecurityTokenExpiredException)
            {
                return fail(TokenParseStatus.Failed, "Token is expired");
            }
            catch (SecurityTokenNotYetValidException)
            {
                return fail(TokenParseStatus.Failed, "Token is not yet valid");
            }
            catch (SecurityTokenInvalidLifetimeException)
            {
                return fail(TokenParseStatus.Failed, "Token has invalid lifetime");
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                return fail(TokenParseStatus.Failed, "Token has invalid audience");
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                return fail(TokenParseStatus.Failed, "Token has invalid issuer");
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                logger.Error("Token signature key not found");

                return fail(TokenParseStatus.Failed, "Token has invalid signature");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return fail(TokenParseStatus.Failed, "Token has invalid signature");
            }
            catch (Exception exception)
            {
                logger.Error($"Token validation failed: {exception}");

                return fail(TokenParseStatus.BadSource, "Token is invalid");
            }
        }

        private IStatusResult<TokenParseStatus, IdBaseToken> fail(TokenParseStatus status, string error) =>
            Result.Status<TokenParseStatus, IdBaseToken>(TokenParseStatus.BadSource, null).Error(error);
    }
}