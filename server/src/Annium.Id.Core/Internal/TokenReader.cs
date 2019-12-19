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
        private readonly AuthOptions authOptions;
        private readonly Func<Instant> getInstant;
        private readonly ILogger logger;

        public TokenReader(
            AuthOptions authOptions,
            Func<Instant> getInstant,
            ILogger<TokenReader> logger
        )
        {
            using (var s = File.OpenRead(authOptions.PublicKeyFile))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }

            this.authOptions = authOptions;
            this.getInstant = getInstant;
            this.logger = logger;
        }

        public IStatusResult<TokenReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return fail(TokenReadStatus.BadSource, "Token is not valid JWT");

            var tvp = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = Constants.Issuer,
                ValidateIssuerSigningKey = true,
            };

            if (options.ValidateAudience)
            {
                tvp.ValidateAudience = true;
                tvp.ValidAudience = authOptions.Audience;
            }
            else
            {
                tvp.ValidateAudience = false;
            }

            if (options.AllowedExpiration == Duration.Zero)
            {
                tvp.ClockSkew = Duration.FromSeconds(5).ToTimeSpan();
                tvp.RequireExpirationTime = true;
                tvp.ValidateLifetime = true;
            }

            try
            {
                handler.ValidateToken(tokenString, tvp, out var securityToken);
                var jwt = (JwtSecurityToken)securityToken;
                if (options.AllowedExpiration != Duration.Zero)
                {
                    var now = getInstant().ToDateTimeUtc();
                    if (jwt.ValidFrom > now)
                        return fail(TokenReadStatus.Failed, "Token is not yet valid");

                    var allowedExpiration = now - options.AllowedExpiration.ToTimeSpan();
                    if (jwt.ValidTo < allowedExpiration)
                        return fail(TokenReadStatus.Failed, "Token is expired");
                }

                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Claims.Id);
                if (idClaim is null)
                    return fail(TokenReadStatus.BadSource, "Token id is missing");

                var rawToken = Convert.FromBase64String(idClaim.Value);
                var token = MessagePackSerializer.Deserialize<IdToken>(
                    rawToken,
                    MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray)
                );

                return Result.Status(TokenReadStatus.Ok, token);
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
                Result.Status<TokenReadStatus, IdToken>(status, null!).Error(error);
        }

    }
}