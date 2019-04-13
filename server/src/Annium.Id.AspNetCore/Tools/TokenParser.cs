using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Annium.Data.Operations;
using Annium.Security.Cryptography;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Annium.Id.AspNetCore.Tools
{
    internal class TokenParser : ITokenParser
    {
        private readonly RsaSecurityKey signingKey;

        public TokenParser()
        {
            using(var s = File.OpenRead(Path.Combine("keys", "public.key")))
            {
                var provider = new RSACryptoServiceProvider();
                provider.ImportParameters(new KeyReader().ReadRsaKey(s));
                signingKey = new RsaSecurityKey(provider);
            }
        }

        public(IdToken, IActionResult) ParseToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString))
                return fail(HttpStatusCode.BadRequest, "Token is not valid JWT");

            var tvp = new TokenValidationParameters();
            tvp.ValidateIssuerSigningKey = true;
            tvp.IssuerSigningKey = signingKey;
            tvp.ValidateIssuer = true;
            tvp.ValidIssuer = Constants.Issuer;
            tvp.ValidateAudience = true;
            tvp.ValidAudience = Constants.Audience;
            tvp.ValidateLifetime = true;
            tvp.ClockSkew = TimeSpan.FromSeconds(5);

            try
            {
                handler.ValidateToken(tokenString, tvp, out var token);
                var jwt = (JwtSecurityToken) token;

                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == Constants.IdClaim);
                if (idClaim == null)
                    return fail(HttpStatusCode.Forbidden, "Token id is missing");

                var idToken = LZ4MessagePackSerializer.Deserialize<IdToken>(Convert.FromBase64String(idClaim.Value));

                return (idToken, null);
            }
            catch
            {
                return fail(HttpStatusCode.Forbidden, "Token is invalid");
            }
        }

        private(IdToken, IActionResult) fail(HttpStatusCode statusCode, string error) =>
            (null, new ObjectResult(Result.Failure().Error(error)) { StatusCode = (int) statusCode });
    }
}