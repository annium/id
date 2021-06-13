using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Annium.Core.Runtime.Time;
using Annium.Data.Operations;
using Annium.Logging.Abstractions;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Annium.Id.Core.Internal
{
    internal class TokenReader : ITokenReader, ILogSubject
    {
        public ILogger Logger { get; }
        private readonly RsaSecurityKey _signingKey;
        private readonly AuthOptions _authOptions;
        private readonly ITimeProvider _timeProvider;

        public TokenReader(
            AuthOptions authOptions,
            ITimeProvider timeProvider,
            ILogger<TokenReader> logger
        )
        {
            using (var s = File.OpenRead(authOptions.PublicKeyFile))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                _signingKey = new RsaSecurityKey(provider);
            }

            _authOptions = authOptions;
            _timeProvider = timeProvider;
            Logger = logger;
        }

        public IStatusResult<TokenReadStatus, IdToken> ReadToken(string tokenString, TokenReadOptions options)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return Fail(TokenReadStatus.BadSource, "Token is not valid JWT");

            var tvp = GetTokenValidationParameters(options);

            try
            {
                handler.ValidateToken(tokenString, tvp, out var securityToken);
                var jwt = (JwtSecurityToken) securityToken;
                if (!options.ValidateExpiration)
                {
                    var now = _timeProvider.Now.ToDateTimeUtc();
                    if (jwt.ValidFrom > now)
                        return Fail(TokenReadStatus.Failed, "Token is not yet valid");
                }

                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Claims.Id);
                if (idClaim is null)
                    return Fail(TokenReadStatus.BadSource, "Token id is missing");

                var rawToken = Convert.FromBase64String(idClaim.Value);
                var token = MessagePackSerializer.Deserialize<IdToken>(
                    rawToken,
                    MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray)
                );

                return Result.Status(TokenReadStatus.Ok, token);
            }
            catch (Exception exception)
            {
                var (status, error) = HandleValidationFailure(exception);

                return Fail(status, error);
            }
        }

        private TokenValidationParameters GetTokenValidationParameters(TokenReadOptions options)
        {
            var tvp = new TokenValidationParameters
            {
                IssuerSigningKey = _signingKey,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = Constants.Issuer,
                ValidateIssuerSigningKey = true
            };

            if (options.ValidateAudience)
            {
                tvp.ValidateAudience = true;
                tvp.ValidAudience = _authOptions.Audience.ToString();
            }
            else
            {
                tvp.ValidateAudience = false;
            }

            if (options.ValidateExpiration)
            {
                tvp.ClockSkew = Duration.FromSeconds(5).ToTimeSpan();
                tvp.RequireExpirationTime = true;
                tvp.ValidateLifetime = true;
            }
            else
            {
                tvp.RequireExpirationTime = false;
                tvp.ValidateLifetime = false;
            }

            return tvp;
        }

        private ValueTuple<TokenReadStatus, string> HandleValidationFailure(Exception exception)
        {
            return exception switch
            {
                SecurityTokenDecompressionFailedException _   => (TokenReadStatus.Failed, "Token decompression failed"),
                SecurityTokenEncryptionKeyNotFoundException _ => Log(TokenReadStatus.Failed, "Token decryption failed", "Token encryption key not found"),
                SecurityTokenDecryptionFailedException _      => (TokenReadStatus.Failed, "Token decryption failed"),
                SecurityTokenNoExpirationException _          => (TokenReadStatus.Failed, "Token has no expiration claim"),
                SecurityTokenExpiredException _               => (TokenReadStatus.Failed, "Token is expired"),
                SecurityTokenNotYetValidException _           => (TokenReadStatus.Failed, "Token is not yet valid"),
                SecurityTokenInvalidLifetimeException _       => (TokenReadStatus.Failed, "Token has invalid lifetime"),
                SecurityTokenInvalidAudienceException _       => (TokenReadStatus.Failed, "Token has invalid audience"),
                SecurityTokenInvalidIssuerException _         => (TokenReadStatus.Failed, "Token has invalid issuer"),
                SecurityTokenSignatureKeyNotFoundException _  => Log(TokenReadStatus.Failed, "Token has invalid signature", "Token signature key not found"),
                SecurityTokenInvalidSignatureException _      => (TokenReadStatus.Failed, "Token has invalid signature"),
                _                                             => Log(TokenReadStatus.BadSource, "Token is invalid", $"Token validation failed: {exception}")
            };
        }


        private ValueTuple<TokenReadStatus, string> Log(TokenReadStatus status, string error, string message)
        {
            this.Error(message);

            return (status, error);
        }

        private IStatusResult<TokenReadStatus, IdToken> Fail(TokenReadStatus status, string error)
        {
            return Result.Status<TokenReadStatus, IdToken>(status, null!).Error(error);
        }
    }
}