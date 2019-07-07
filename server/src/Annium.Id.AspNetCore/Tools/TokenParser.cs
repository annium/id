using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Annium.Data.Operations;
using Annium.Id.AspNetCore.Pipeline;
using Annium.Logging.Abstractions;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Annium.Id.AspNetCore.Tools
{
    internal class TokenParser
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

        public(object, IActionResult) ParseToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return fail(HttpStatusCode.BadRequest, "Token is not valid JWT");

            var tvp = new TokenValidationParameters();
            tvp.ClockSkew = TimeSpan.FromSeconds(5);
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
                handler.ValidateToken(tokenString, tvp, out var token);
                var jwt = (JwtSecurityToken) token;

                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Claims.Id);
                if (idClaim == null)
                    return fail(HttpStatusCode.Forbidden, "Token id is missing");

                if (jwt.Audiences.Contains(Constants.BaseAudience))
                    return (LZ4MessagePackSerializer.Deserialize<IdBaseToken>(Convert.FromBase64String(idClaim.Value)), null);

                return (LZ4MessagePackSerializer.Deserialize<IdAppToken>(Convert.FromBase64String(idClaim.Value)), null);
            }
            catch (SecurityTokenDecompressionFailedException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token decompression failed");
            }
            catch (SecurityTokenEncryptionKeyNotFoundException)
            {
                logger.Error("Token encryption key not found");

                return fail(HttpStatusCode.Unauthorized, "Token decryption failed");
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token decryption failed");
            }
            catch (SecurityTokenNoExpirationException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token has no expiration claim");
            }
            catch (SecurityTokenExpiredException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token is expired");
            }
            catch (SecurityTokenNotYetValidException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token is not yet valid");
            }
            catch (SecurityTokenInvalidLifetimeException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token has invalid lifetime");
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token has invalid audience");
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token has invalid issuer");
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                logger.Error("Token signature key not found");

                return fail(HttpStatusCode.Unauthorized, "Token has invalid signature");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return fail(HttpStatusCode.Unauthorized, "Token has invalid signature");
            }
            catch (Exception exception)
            {
                logger.Error($"Token validation failed: {exception}");

                return fail(HttpStatusCode.BadRequest, "Token is invalid");
            }
        }

        private(IdBaseToken, IActionResult) fail(HttpStatusCode statusCode, string error) =>
            (null, new ObjectResult(Result.Failure().Error(error)) { StatusCode = (int) statusCode });
    }
}