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

namespace Annium.Id.Core.Internal
{
    internal class TokenReader : ITokenReader
    {
        private readonly RsaSecurityKey signingKey;
        private readonly AuthOptions options;
        private readonly ILogger logger;

        public TokenReader(
            AuthOptions options,
            ILogger<TokenReader> logger
        )
        {
            using (var s = File.OpenRead(options.PublicKeyFile))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }

            this.options = options;
            this.logger = logger;
        }

        public IStatusResult<TokenReadStatus, IdToken> ReadToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return fail(TokenReadStatus.BadSource, "Token is not valid JWT");

            var tvp = new TokenValidationParameters
            {
                ClockSkew = Duration.FromSeconds(5).ToTimeSpan(),
                IssuerSigningKey = signingKey,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidAudiences = new[] { options.Audience },
                ValidateIssuer = true,
                ValidIssuer = Constants.Issuer,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true
            };

            try
            {
                handler.ValidateToken(tokenString, tvp, out var securityToken);
                var jwt = (JwtSecurityToken)securityToken;

                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Claims.Id);
                if (idClaim == null)
                    return fail(TokenReadStatus.BadSource, "Token id is missing");

                var rawToken = Convert.FromBase64String(idClaim.Value);

                return Result.Status(TokenReadStatus.Ok, LZ4MessagePackSerializer.Deserialize<IdToken>(rawToken));
            }
            catch (SecurityTokenDecompressionFailedException)
            {
                return fail(TokenReadStatus.Failed, "Token decompression failed");
            }
            catch (SecurityTokenEncryptionKeyNotFoundException)
            {
                logger.Error("Token encryption key not found");

                return fail(TokenReadStatus.Failed, "Token decryption failed");
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                return fail(TokenReadStatus.Failed, "Token decryption failed");
            }
            catch (SecurityTokenNoExpirationException)
            {
                return fail(TokenReadStatus.Failed, "Token has no expiration claim");
            }
            catch (SecurityTokenExpiredException)
            {
                return fail(TokenReadStatus.Failed, "Token is expired");
            }
            catch (SecurityTokenNotYetValidException)
            {
                return fail(TokenReadStatus.Failed, "Token is not yet valid");
            }
            catch (SecurityTokenInvalidLifetimeException)
            {
                return fail(TokenReadStatus.Failed, "Token has invalid lifetime");
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                return fail(TokenReadStatus.Failed, "Token has invalid audience");
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                return fail(TokenReadStatus.Failed, "Token has invalid issuer");
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                logger.Error("Token signature key not found");

                return fail(TokenReadStatus.Failed, "Token has invalid signature");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return fail(TokenReadStatus.Failed, "Token has invalid signature");
            }
            catch (Exception exception)
            {
                logger.Error($"Token validation failed: {exception}");

                return fail(TokenReadStatus.BadSource, "Token is invalid");
            }

            static IStatusResult<TokenReadStatus, IdToken> fail(TokenReadStatus status, string error) =>
                Result.Status<TokenReadStatus, IdToken>(TokenReadStatus.BadSource, null!).Error(error);
        }

    }
}